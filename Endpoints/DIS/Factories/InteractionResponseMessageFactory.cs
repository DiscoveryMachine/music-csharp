/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using MUSICLibrary.MUSIC_Messages;
using MUSICLibrary.MUSIC_Messages.MUSIC_Response_Messages;
using Newtonsoft.Json.Linq;
using OpenDis.Core;
using OpenDis.Dis1998;

namespace MUSICLibrary.Endpoints.DIS.Factories
{
    public class InteractionResponseMessageFactory : IDISMUSICMessageFactory
    {
        public MUSICMessage Create(byte[] pdu)
        {
            ActionResponsePdu disPdu = new ActionResponsePdu();
            disPdu.Unmarshal(new DataInputStream(pdu, Endian.Big));

            JObject optionalData = null;
            if (disPdu.NumberOfVariableDatumRecords > 0)
                optionalData = JObject.Parse(PduReader.ReadStringFromDatum(disPdu.VariableDatums[0]));

            return new InteractionResponseMessage
            {
                MUSICHeader = new MUSICHeader(disPdu.ExerciseID),
                OriginID = disPdu.OriginatingEntityID,
                ReceiverID = disPdu.ReceivingEntityID,
                RequestID = disPdu.RequestID,
                RequestStatus = (RequestStatus)disPdu.RequestStatus,
                OptionalData = optionalData,
            };
        }
    }
}
