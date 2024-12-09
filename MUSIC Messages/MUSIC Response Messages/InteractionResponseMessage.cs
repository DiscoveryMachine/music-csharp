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
using System;
using System.Collections.Generic;

namespace MUSICLibrary.MUSIC_Messages.MUSIC_Response_Messages
{
    public class InteractionResponseMessage : MUSICResponseMessage
    {
        public JObject OptionalData { get; set; }

        public InteractionResponseMessage()
        {

        }

        protected InteractionResponseMessage(uint exerciseID, EntityIDRecord originID,
            EntityIDRecord receiverID, uint requestID, RequestStatus status, JObject optionalData = null)
            : base(exerciseID, originID, receiverID, requestID, status)
        {
            OptionalData = optionalData;
        }

        public InteractionResponseMessage(JObject jobj) : base(jobj)
        {
            if (jobj.TryGetValue(MUSICJsonKeys.OPTIONAL_DATA, out JToken optionalDataToken))
                OptionalData = jobj[MUSICJsonKeys.OPTIONAL_DATA].ToObject<JObject>();

            RequestStatus = jobj[MUSICJsonKeys.STATUS].ToObject<RequestStatus>();
        }

        public InteractionResponseMessage(InteractionRequestMessage msg, RequestStatus requestStatus, JObject optionalData = null)
            : base(msg, requestStatus)
        {
            OptionalData = optionalData;
            RequestStatus = requestStatus;
        }

        public override void AcceptVisitor(IMUSICMessageVisitor visitor)
        {
            visitor.VisitInteractionResponse(this);
        }

        public override object Clone()
        {
            return new InteractionResponseMessage
                (
                    MUSICHeader.ExerciseID,
                    (EntityIDRecord)OriginID.Clone(),
                    (EntityIDRecord)ReceiverID.Clone(),
                    RequestID,
                    RequestStatus,
                    (JObject)OptionalData?.DeepClone()
                );
        }

        public override JObject ToJsonObject()
        {
            JObject jobj = new JObject
            {
                { MUSICJsonKeys.HEADER, MUSICHeader.ToJsonObject() },
                { MUSICJsonKeys.ORIGIN_ID, OriginID.ToJsonObject() },
                { MUSICJsonKeys.RECEIVER_ID, ReceiverID.ToJsonObject() },
                { MUSICJsonKeys.REQUEST_ID, RequestID },
                { MUSICJsonKeys.STATUS, (int)RequestStatus }
            };

            if (OptionalData != null)
                jobj.Add(MUSICJsonKeys.OPTIONAL_DATA, OptionalData);

            return jobj;
        }

        public override bool Equals(object obj)
        {
            if (obj is InteractionResponseMessage other)
                return base.Equals(obj) && JObject.DeepEquals(OptionalData, other.OptionalData);

            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 627368575;

            hashCode = hashCode * -1521137895 + EqualityComparer<uint>.Default.GetHashCode(MUSICHeader.ExerciseID);
            hashCode = hashCode * -1521137895 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(OriginID);
            hashCode = hashCode * -1521137895 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(ReceiverID);
            hashCode = hashCode * -1521137895 + EqualityComparer<uint>.Default.GetHashCode(RequestID);
            hashCode = hashCode * -1521137895 + EqualityComparer<RequestStatus?>.Default.GetHashCode(RequestStatus);
            hashCode = hashCode * -1521137895 + EqualityComparer<string>.Default.GetHashCode(OptionalData.ToString());

            return hashCode;
        }

        public override string ToString()
        {
            return ToJsonObject().ToString();
        }

        public static bool operator ==(InteractionResponseMessage message1, InteractionResponseMessage message2)
        {
            return EqualityComparer<InteractionResponseMessage>.Default.Equals(message1, message2);
        }

        public static bool operator !=(InteractionResponseMessage message1, InteractionResponseMessage message2)
        {
            return !(message1 == message2);
        }
    }
}
