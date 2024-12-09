//=====================================================================
// DISCOVERY MACHINE, INC. PROPRIETARY INFORMATION
//
// This software is supplied under the terms of a license agreement
// or nondisclosure agreement with Discovery Machine, Inc. and may
// not be copied or disclosed except in accordance with the terms of
// that agreement.
//
// Copyright 2022-23 Discovery Machine, Inc. All Rights Reserved.
//=====================================================================

using MUSICLibrary.Interfaces;
using MUSICLibrary.MUSIC_Messages.MUSIC_Request_Messages;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MUSICLibrary.MUSIC_Messages.MUSIC_Response_Messages
{
    public class ControlTransferResponseMessage : MUSICResponseMessage
    {
        protected ControlTransferResponseMessage(uint exerciseID, EntityIDRecord originID,
            EntityIDRecord receiverID, uint requestID, RequestStatus status)
            : base(exerciseID, originID, receiverID, requestID, status)
        {

        }

        public ControlTransferResponseMessage(JObject jobj) : base(jobj) { }

        public ControlTransferResponseMessage(ControlTransferRequestMessage msg, RequestStatus status)
            : base(msg, status)
        {

        }

        public override void AcceptVisitor(IMUSICMessageVisitor visitor)
        {
            visitor.VisitControlTransferResponse(this);
        }

        public override object Clone()
        {
            return new ControlTransferResponseMessage
                (
                    MUSICHeader.ExerciseID,
                    (EntityIDRecord)OriginID.Clone(),
                    (EntityIDRecord)ReceiverID.Clone(),
                    RequestID,
                    RequestStatus
                );
        }

        public override JObject ToJsonObject()
        {
            return new JObject
            {
                { MUSICJsonKeys.HEADER, MUSICHeader.ToJsonObject() },
                { MUSICJsonKeys.ORIGIN_ID, OriginID.ToJsonObject() },
                { MUSICJsonKeys.RECEIVER_ID, ReceiverID.ToJsonObject() },
                { MUSICJsonKeys.REQUEST_ID, RequestID },
                { MUSICJsonKeys.STATUS, (int)RequestStatus }
            };
        }

        public override bool Equals(object obj)
        {
            if (obj is ControlTransferResponseMessage other)
                if (base.Equals(other))
                    return true;

            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 500213460;

            hashCode = hashCode * -1521134265 + EqualityComparer<uint>.Default.GetHashCode(MUSICHeader.ExerciseID);
            hashCode = hashCode * -1521134265 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(OriginID);
            hashCode = hashCode * -1521134265 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(ReceiverID);
            hashCode = hashCode * -1521134265 + EqualityComparer<uint>.Default.GetHashCode(RequestID);
            hashCode = hashCode * -1521134265 + EqualityComparer<RequestStatus?>.Default.GetHashCode(RequestStatus);

            return hashCode;
        }

        public static bool operator ==(ControlTransferResponseMessage message1, ControlTransferResponseMessage message2)
        {
            return EqualityComparer<ControlTransferResponseMessage>.Default.Equals(message1, message2);
        }

        public static bool operator !=(ControlTransferResponseMessage message1, ControlTransferResponseMessage message2)
        {
            return !(message1 == message2);
        }
    }
}
