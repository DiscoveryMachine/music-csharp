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

using MUSICLibrary.Endpoints;
using MUSICLibrary.Interfaces;
using MUSICLibrary.MUSIC_Messages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MUSICLibrary
{
    public class MUSIC : IMUSICLibrary, IMUSICTransmitter
    {
        protected IInternalMessageVisitor InternalVisitor { get => Config.InternalMessageVisitor; }
        protected IMUSICMessageVisitor ExternalVisitor { get => Config.ExternalMessageVisitor; }
        protected IMessageFilter MessageFilter { get => Config.MessageFilter; }

        protected LibraryConfiguration Config { get; }
        protected EntityIDRecord SimulationID { get; }
        protected Dictionary<Type, MUSICEndpoint> Endpoints { get; }

        protected uint ExerciseID { get => Config.ExerciseID; }

        protected bool subscribed;

        public MUSIC(LibraryConfiguration config)
        {
            Config = config;
            SimulationID = new EntityIDRecord(config.SiteAndAppID, 0);
            Endpoints = new Dictionary<Type, MUSICEndpoint>();
            subscribed = false;
        }

        public IMUSICLibrary InitializeEndpoint(Type type)
        {
            if (!subscribed)
            {
                MUSICEndpoint endpoint = (MUSICEndpoint)Activator.CreateInstance(type, Config);
                Endpoints.Add(type, endpoint);
            }
            else
                throw new EndpointInitializationException("Cannot initialize endpoints while subscribed to MUSIC traffic.");

            return this;
        }

        public IMUSICLibrary RemoveMUSICEndpoint(Type type)
        {
            if (Endpoints.ContainsKey(type))
            {
                MUSICEndpoint endpoint = Endpoints[type];

                if (endpoint.Subscribed)
                    endpoint.UnsubscribeFromMUSIC();
            }

            Endpoints.Remove(type);
            return this;
        }

        public void SubscribeToMUSIC()
        {
            if (!subscribed)
                Endpoints.ToList().ForEach((kvPair) => kvPair.Value.SubscribeToMUSIC());

            subscribed = true;
        }

        public void UnsubscribeToMUSIC()
        {
            if (subscribed)
                Endpoints.ToList().ForEach((kvPair) => kvPair.Value.UnsubscribeFromMUSIC());

            subscribed = false;
        }

        public void Transmit(MUSICMessage message)
        {
            Endpoints.ToList().ForEach((kvPair) => kvPair.Value.Transmit(message));
        }

        public bool IsSubscribed()
        {
            return subscribed;
        }

        public uint GetExerciseID()
        {
            return ExerciseID;
        }

        public MUSICEndpoint GetEndpoint(Type type)
        {
            return Endpoints[type];
        }

        public SiteAndAppID GetSiteAndAppID()
        {
            return SimulationID.GetSiteAndApp();
        }

        public IConstruct CreateLocalConstruct(IConstructCreateInfo createInfo)
        {
            return InternalVisitor.CreateLocalConstruct(createInfo);
        }

        public class EndpointInitializationException : Exception
        {
            public EndpointInitializationException(string message) : base(message)
            {

            }
        }
    }
}
 