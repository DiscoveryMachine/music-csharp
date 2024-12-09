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

namespace MUSICLibrary.MUSIC_Messages.Targeted_MUSIC_Messages
{
    public class ControlReclamationMessage :TargetedMUSICMessage
    {
        public EntityIDRecord TargetConstruct { get; set; }

        public ControlReclamationMessage(uint exerciseID, 
            EntityIDRecord originID, EntityIDRecord recieverID, EntityIDRecord targetConstruct) : base(exerciseID, originID, recieverID)
        {
            TargetConstruct = targetConstruct;
        }

        public ControlReclamationMessage(JObject obj) : base(obj)
        {
            TargetConstruct = obj.GetValue(MUSICJsonKeys.TARGET_CONSTRUCT).ToObject<EntityIDRecord>();
        }

        public override void AcceptVisitor(IMUSICMessageVisitor visitor)
        {
            visitor.VisitControlReclamation(this);
        }

        public override object Clone()
        {
             return new ControlReclamationMessage
                (
                    MUSICHeader.ExerciseID,
                    (EntityIDRecord)OriginID.Clone(),
                    (EntityIDRecord)ReceiverID.Clone(),
                    (EntityIDRecord)TargetConstruct.Clone()
                );
        }

        public override JObject ToJsonObject()
        {
            return new JObject
            {
                { MUSICJsonKeys.HEADER, MUSICHeader.ToJsonObject() },
                { MUSICJsonKeys.ORIGIN_ID, OriginID.ToJsonObject() },
                { MUSICJsonKeys.RECEIVER_ID, ReceiverID.ToJsonObject() },
                { MUSICJsonKeys.TARGET_CONSTRUCT, TargetConstruct.ToJsonObject() }
            };
        }

        public override bool Equals(object obj)
        {
            var message = obj as ControlReclamationMessage;
            return message != null &&
                   base.Equals(obj) &&
                   EqualityComparer<EntityIDRecord>.Default.Equals(TargetConstruct, message.TargetConstruct);
        }

        public override int GetHashCode()
        {
            var hashCode = 2075079299;

            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(TargetConstruct);

            return hashCode;
        }

        public static bool operator ==(ControlReclamationMessage message1, ControlReclamationMessage message2)
        {
            return EqualityComparer<ControlReclamationMessage>.Default.Equals(message1, message2);
        }

        public static bool operator !=(ControlReclamationMessage message1, ControlReclamationMessage message2)
        {
            return !(message1 == message2);
        }
    }
}
