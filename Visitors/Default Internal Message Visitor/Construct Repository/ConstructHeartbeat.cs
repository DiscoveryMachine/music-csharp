/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

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
