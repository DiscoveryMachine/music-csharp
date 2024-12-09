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
    public class PrimaryControlResponseMessage : MUSICResponseMessage
    {
        protected PrimaryControlResponseMessage(uint exerciseID, EntityIDRecord originID,
            EntityIDRecord receiverID, uint requestID, RequestStatus status)
            : base(exerciseID, originID, receiverID, requestID, status)
        {

        }

        public PrimaryControlResponseMessage(JObject jobj) : base(jobj)
        {

        }

        public PrimaryControlResponseMessage(PrimaryControlRequestMessage msg, RequestStatus status) : base(msg, status)
        {

        }

        public override void AcceptVisitor(IMUSICMessageVisitor visitor)
        {
            visitor.VisitPrimaryControlResponse(this);
        }

        public override object Clone()
        {
            return new PrimaryControlResponseMessage
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
            if (obj is PrimaryControlResponseMessage other)
                if (base.Equals(other))
                    return true;

            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 210445777;

            hashCode = hashCode * -1521004777 + EqualityComparer<uint>.Default.GetHashCode(MUSICHeader.ExerciseID);
            hashCode = hashCode * -1521004777 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(OriginID);
            hashCode = hashCode * -1521004777 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(ReceiverID);
            hashCode = hashCode * -1521004777 + EqualityComparer<uint>.Default.GetHashCode(RequestID);
            hashCode = hashCode * -1521004777 + EqualityComparer<RequestStatus?>.Default.GetHashCode(RequestStatus);

            return hashCode;
        }

        public static bool operator ==(PrimaryControlResponseMessage message1, PrimaryControlResponseMessage message2)
        {
            return EqualityComparer<PrimaryControlResponseMessage>.Default.Equals(message1, message2);
        }

        public static bool operator !=(PrimaryControlResponseMessage message1, PrimaryControlResponseMessage message2)
        {
            return !(message1 == message2);
        }
    }
}
