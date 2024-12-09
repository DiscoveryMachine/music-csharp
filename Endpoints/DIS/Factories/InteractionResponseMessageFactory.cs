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
