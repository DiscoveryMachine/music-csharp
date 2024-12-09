//=====================================================================
// DISCOVERY MACHINE, INC. PROPRIETARY INFORMATION
//
// This software is supplied under the terms of a license agreement
// or nondisclosure agreement with Discovery Machine, Inc. and may
// not be copied or disclosed except in accordance with the terms of
// that agreement.
//
// Copyright 2023 Discovery Machine, Inc. All Rights Reserved.
//=====================================================================

using MUSICLibrary.MUSIC_Messages;
using MUSICLibrary.MUSIC_Messages.Targeted_MUSIC_Messages;
using OpenDis.Core;
using OpenDis.Dis1998;

namespace MUSICLibrary.Endpoints.DIS.Factories
{
    public class RequestSimulationTimeMessageFactory : IDISMUSICMessageFactory
    {
        public MUSICMessage Create(byte[] pdu)
        {
            SetDataPdu disPdu = new SetDataPdu();
            disPdu.Unmarshal(new DataInputStream(pdu, Endian.Big));

            return new RequestSimulationTimeMessage
            {
                MUSICHeader = new MUSICHeader(disPdu.ExerciseID),
                OriginID = disPdu.OriginatingEntityID,
                ReceiverID = disPdu.ReceivingEntityID
            };
        }
    }
}
