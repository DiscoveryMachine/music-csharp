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
using OpenDis.Core;

namespace MUSICLibrary.Endpoints.DIS.Factories
{
    public class StateFieldMessageFactory : IDISMUSICMessageFactory
    {
        public StateFieldMessageFactory()
        {
        }

        public MUSICMessage Create(byte[] pdu)
        {
            StateFieldPdu musicPdu = new StateFieldPdu();
            musicPdu.Unmarshal(new DataInputStream(pdu, Endian.Big));

            return new StateFieldMessage
            {
                MUSICHeader = new MUSICHeader(musicPdu.ExerciseID),
                OriginID = musicPdu.ConstructID,
                StateDataObject = musicPdu.Payload,
            };
        }
    }
}
