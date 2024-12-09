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
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MUSICLibrary.MUSIC_Messages.MUSIC_Request_Messages
{
    public class InteractionRequestMessage : MUSICRequestMessage
    {
        public string InteractionName { get; set; }
        public InteractionType InteractionType { get; set; }
        public JObject InteractionData { get; set; }

        public InteractionRequestMessage()
        {

        }

        public InteractionRequestMessage(uint exerciseID, EntityIDRecord originID, EntityIDRecord receiverID,
            uint requestID, string interactionName, InteractionType interactionType, JObject interactionData = null)
            : base(exerciseID, originID, receiverID, requestID)
        {
            InteractionName = interactionName;
            InteractionType = interactionType;
            InteractionData = interactionData;
        }

        public InteractionRequestMessage(JObject jobj)
            : base(jobj)
        {
            InteractionName = jobj[MUSICJsonKeys.INTERACTION_NAME].ToObject<string>();
            InteractionType = jobj[MUSICJsonKeys.INTERACTION_TYPE].ToObject<InteractionType>();
            InteractionData = jobj[MUSICJsonKeys.INTERACTION_DATA].ToObject<JObject>();
        }

        public override void AcceptVisitor(IMUSICMessageVisitor visitor)
        {
            visitor.VisitInteractionRequest(this);
        }

        public override object Clone()
        {
            return new InteractionRequestMessage(
                    MUSICHeader.ExerciseID,
                    (EntityIDRecord)OriginID.Clone(),
                    (EntityIDRecord)ReceiverID.Clone(),
                    RequestID,
                    string.Copy(InteractionName),
                    InteractionType,
                    (JObject)InteractionData.DeepClone()
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
                { MUSICJsonKeys.INTERACTION_NAME, InteractionName },
                { MUSICJsonKeys.INTERACTION_TYPE, (uint)InteractionType },
                { MUSICJsonKeys.INTERACTION_DATA, InteractionData }
            };
        }

        public override bool Equals(object obj)
        {
            var message = obj as InteractionRequestMessage;
            return message != null &&
                   base.Equals(obj) &&
                   InteractionName == message.InteractionName &&
                   InteractionType == message.InteractionType &&
                   JObject.DeepEquals(InteractionData, message.InteractionData);
        }

        public override int GetHashCode()
        {
            var hashCode = 848132228;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + InteractionName.GetHashCode();
            hashCode = hashCode * -1521134295 + InteractionType.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(InteractionData.ToString());
            return hashCode;
        }

        public static bool operator ==(InteractionRequestMessage message1, InteractionRequestMessage message2)
        {
            return EqualityComparer<InteractionRequestMessage>.Default.Equals(message1, message2);
        }

        public static bool operator !=(InteractionRequestMessage message1, InteractionRequestMessage message2)
        {
            return !(message1 == message2);
        }
    }
}
