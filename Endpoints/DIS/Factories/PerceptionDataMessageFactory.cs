/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

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
