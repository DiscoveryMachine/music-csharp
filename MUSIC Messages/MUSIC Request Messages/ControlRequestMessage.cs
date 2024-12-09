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
    public class ControlRequestMessage : MUSICRequestMessage
    {
        public EntityIDRecord TargetConstruct { get; set; }
        public JObject Context { get; set; }

        public ControlRequestMessage()
        {

        }

        public ControlRequestMessage(uint exerciseID, EntityIDRecord originID,
            EntityIDRecord receiverID, uint requestID, EntityIDRecord targetConstruct,
            JObject context = null)
            : base(exerciseID, originID, receiverID, requestID)
        {
            TargetConstruct = targetConstruct;
            Context = context;
        }

        public ControlRequestMessage(JObject jobj)
            : base(jobj)
        {
            TargetConstruct = jobj[MUSICJsonKeys.TARGET_CONSTRUCT].ToObject<EntityIDRecord>();
            Context = jobj[MUSICJsonKeys.CONTEXT].ToObject<JObject>();
        }

        public override void AcceptVisitor(IMUSICMessageVisitor visitor)
        {
            visitor.VisitControlRequest(this);
        }

        public override object Clone()
        {
            return new ControlRequestMessage
                (
                    MUSICHeader.ExerciseID, 
                    (EntityIDRecord)OriginID.Clone(),
                    (EntityIDRecord)ReceiverID.Clone(),
                    RequestID,
                    (EntityIDRecord)TargetConstruct.Clone(),
                    (JObject)Context.DeepClone()
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
                { MUSICJsonKeys.TARGET_CONSTRUCT, TargetConstruct.ToJsonObject() },
                { MUSICJsonKeys.CONTEXT, Context }
            };
        }
        public override bool Equals(object obj)
        {
            if (obj is ControlRequestMessage)
                return 
                    base.Equals(obj) &&
                    TargetConstruct.Equals(((ControlRequestMessage)obj).TargetConstruct) &&
                    JObject.DeepEquals(Context, ((ControlRequestMessage)obj).Context);

            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = -326822633;
            hashCode = hashCode * -1521134295 + EqualityComparer<uint>.Default.GetHashCode(MUSICHeader.ExerciseID);
            hashCode = hashCode * -1521134295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(OriginID);
            hashCode = hashCode * -1521134295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(ReceiverID);
            hashCode = hashCode * -1521134295 + EqualityComparer<uint>.Default.GetHashCode(RequestID);
            hashCode = hashCode * -1521134295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(TargetConstruct);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Context.ToString());
            return hashCode;
        }

        public static bool operator ==(ControlRequestMessage message1, ControlRequestMessage message2)
        {
            return EqualityComparer<ControlRequestMessage>.Default.Equals(message1, message2);
        }

        public static bool operator !=(ControlRequestMessage message1, ControlRequestMessage message2)
        {
            return !(message1 == message2);
        }
    }
}
