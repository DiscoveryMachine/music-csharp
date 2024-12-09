//=====================================================================
// DISCOVERY MACHINE, INC. PROPRIETARY INFORMATION
//
// This software is supplied under the terms of a license agreement
// or nondisclosure agreement with Discovery Machine, Inc. and may
// not be copied or disclosed except in accordance with the terms of
// that agreement.
//
// Copyright 2022-23 Discovery Machine, Inc. All Rights Reserved.
//=====================================================================

using System;
using System.Collections.Generic;
using MUSICLibrary.Interfaces;
using MUSICLibrary.MUSIC_Messages;
using MUSICLibrary.MUSIC_Messages.Construct_Data;

namespace MUSICLibrary.Visitors.Default_Internal_Message_Visitor.Construct_Repository
{
    public class ConstructHeartbeat : IBatchOperation
    {
        public IMUSICTransmitter Transmitter { get; internal set; }

        public ConstructHeartbeat(IMUSICTransmitter transmitter)
        {
            Transmitter = transmitter;
        }

        public void ApplyTo(Dictionary<EntityIDRecord, IConstruct> batch)
        {
            lock(batch)
            {
                var batchCopy = new List<IConstruct>(batch.Values);
                foreach (IConstruct construct in batchCopy)
                {
                    if (construct.IsRemote())
                        throw new InvalidOperationException();

                    ConstructDataMessage constructData = construct.GetConstructData();
                    Transmitter.Transmit(constructData);

                    StateFieldMessage stateFieldData = construct.GetStateFieldData();
                    Transmitter.Transmit(stateFieldData);
                }
            }
        }
    }
}
