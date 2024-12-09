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
