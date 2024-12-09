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
using MUSICLibrary.MUSIC_Messages.PerceptionData;
using System.Collections.Generic;

namespace MUSICLibrary.Endpoints.DIS.Factories
{
    public class PerceptionDataMessageFactory : IDISMUSICMessageFactory
    {
        public MUSICMessage Create(byte[] pdu)
        {
            PduReader reader = new PduReader(pdu);
            List<PerceptionRecord> perceptions = new List<PerceptionRecord>();

            var length = pdu[PduReader.PERCEPTIONS_LENGTH_OFFSET];
            int offset = PduReader.PERCEPTIONS_OFFSET;

            for (int i = 0; i < length; i++)
                perceptions.Add(reader.ReadPerceptionRecord(ref offset));

            return new PerceptionDataMessage(reader.ReadExerciseID(), reader.ReadOriginID(), perceptions);
        }
    }
}
