/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

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
