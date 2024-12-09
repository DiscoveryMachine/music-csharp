//=====================================================================
// DISCOVERY MACHINE, INC. PROPRIETARY INFORMATION
//
// This software is supplied under the terms of a license agreement
// or nondisclosure agreement with Discovery Machine, Inc. and may
// not be copied or disclosed except in accordance with the terms  of
// that agreement.
//
// Copyright 2022-23 Discovery Machine, Inc. All Rights Reserved.
//=====================================================================

using System;
using MUSICLibrary.Interfaces;
using MUSICLibrary.MUSIC_Messages;
using MUSICLibrary.Visitors.Default_Internal_Message_Visitor.Construct_Repository.Batcher;

namespace MUSICLibrary.Visitors.Default_Internal_Message_Visitor.Construct_Repository
{
    public class ConstructRepository : IConstructRepository
    {
        public IMUSICTransmitter Transmitter { get; }

        protected ConstructBatches LocalConstructs { get; set; }
        protected ConstructBatches RemoteConstructs { get; set; }

        private IConstructBatcher LocalBatcher { get; set; }
        private IConstructBatcher RemoteBatcher { get; set; } 

        private const string ENTITY_ID_ZERO_ERROR_MSG_FORMAT = "[Warning]: Attempted to {0}, but the EntityID was 0. An EntityIDRecord.EntityID of 0 is a reserved value for an " +
            "EntityIDRecord to refer to the simulation itself rather than an individual construct.";

        public ConstructRepository(IMUSICTransmitter transmitter, RepositoryConfiguration repositoryConfig)
        {
            LocalConstructs = new ConstructBatches(repositoryConfig?.LocalBatchCount ?? 1);
            RemoteConstructs = new ConstructBatches(repositoryConfig?.RemoteBatchCount ?? 1);
            LocalBatcher = repositoryConfig?.LocalBatcher;
            RemoteBatcher = repositoryConfig?.RemoteBatcher;
            Transmitter = transmitter;
        }

        public IConstruct this[EntityIDRecord eid]
        {
            get
            {
                if (eid.EntityID == 0)
                    throw new EntityIDIsZeroException(string.Format(ENTITY_ID_ZERO_ERROR_MSG_FORMAT, "get a construct by its id"));

                try
                {
                    return RemoteConstructs.GetConstructByID(eid);
                }
                catch (ConstructNotFoundException)
                {
                    // No op to check local constructs
                }

                try
                {
                    return LocalConstructs.GetConstructByID(eid);
                }
                catch (ConstructNotFoundException)
                {
                    throw new ConstructNotFoundException("Construct not found for ConstructID: " + eid.ToJsonObject().ToString());
                }
            }
            set
            {
                if (eid.EntityID == 0)
                    throw new EntityIDIsZeroException(string.Format(ENTITY_ID_ZERO_ERROR_MSG_FORMAT, "add an entry into the construct map with an id"));

                if (value.IsRemote())
                {
                    RemoteConstructs.Add(value);
                }
                else
                {
                    LocalConstructs.Add(value);
                }
            }
        }

        public void AddConstruct(IConstruct construct)
        {
            this[construct.GetID()] = construct;
        }

        public IConstruct GetConstructByID(EntityIDRecord eid)
        {
            return this[eid];
        }

        public int GetConstructCount()
        {
            return (int) (LocalConstructs.Count + RemoteConstructs.Count);
        }

        public int GetLocalConstructCount()
        {
            return (int) LocalConstructs.Count;
        }

        public int GetRemoteConstructCount()
        {
            return (int) RemoteConstructs.Count;
        }

        public void StartBatchThreads()
        { 
            LocalBatcher.StartBatchThreads(new ConstructHeartbeat(Transmitter), LocalConstructs);
            LocalBatcher.StartBatchThreads(new ConstructCleaner(), LocalConstructs);
            RemoteBatcher.StartBatchThreads(new ConstructDeadReckoning(), RemoteConstructs);
        }

        public bool ConstructExists(EntityIDRecord eid)
        {
            try
            {
                RemoteConstructs.GetConstructByID(eid);
                return true;
            }
            catch(ConstructNotFoundException)
            {
                // No op to check local constructs
            }

            try
            {
                LocalConstructs.GetConstructByID(eid);
                return true;
            }
            catch (ConstructNotFoundException)
            {
                return false;
            }
        }

        public class EntityIDIsZeroException : Exception
        {
            public EntityIDIsZeroException(string message) : base(message) { }
        }

        public class ConstructNotFoundException : Exception
        {
            public ConstructNotFoundException() { }
            public ConstructNotFoundException(string message) { }
        }
    }
}
