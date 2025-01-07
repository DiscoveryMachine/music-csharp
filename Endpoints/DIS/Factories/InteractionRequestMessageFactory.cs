/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using MUSICLibrary.MUSIC_Messages;
using MUSICLibrary.MUSIC_Messages.MUSIC_Request_Messages;
using Newtonsoft.Json.Linq;
using OpenDis.Core;
using OpenDis.Dis1998;

namespace MUSICLibrary.Endpoints.DIS.Factories
{
    public class InteractionRequestMessageFactory : IDISMUSICMessageFactory
    {
        public MUSICMessage Create(byte[] pdu)
        {
            ActionRequestPdu disPdu = new ActionRequestPdu();
            disPdu.Unmarshal(new DataInputStream(pdu, Endian.Big));
            var interactionDataJson = PduReader.ReadStringFromDatum(disPdu.VariableDatums[1]);
            JObject interactionData = JObject.Parse(interactionDataJson);

            return new InteractionRequestMessage
            {
                MUSICHeader = new MUSICHeader(disPdu.ExerciseID),
                OriginID = disPdu.OriginatingEntityID,
                ReceiverID = disPdu.ReceivingEntityID,
                RequestID = disPdu.RequestID,
                InteractionName = PduReader.ReadStringFromDatum(disPdu.VariableDatums[0]),
                InteractionType = (InteractionType)disPdu.VariableDatums[0].VariableDatumID,
                InteractionData = interactionData,
            };
        }
    }
}
