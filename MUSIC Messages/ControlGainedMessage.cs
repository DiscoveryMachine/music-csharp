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

namespace MUSICLibrary.MUSIC_Messages
{
    public class ControlGainedMessage : MUSICMessage
    {
        public EntityIDRecord ReceiverID { get; set; }
        public EntityIDRecord GainedControlOf { get; private set; }

        public ControlGainedMessage(uint exerciseID, EntityIDRecord originID, 
            EntityIDRecord receiver, EntityIDRecord gainedControlOf)
            : base(exerciseID, originID)
        {
            ReceiverID = receiver;
            GainedControlOf = gainedControlOf;
        }

        public ControlGainedMessage(JObject obj) :
            base(obj.GetValue(MUSICJsonKeys.HEADER).ToObject<MUSICHeader>(),
                obj.GetValue(MUSICJsonKeys.ORIGIN_ID).ToObject<EntityIDRecord>())
        {
            ReceiverID = obj.GetValue(MUSICJsonKeys.RECEIVER_ID).
                ToObject<EntityIDRecord>();

            GainedControlOf = obj.GetValue(MUSICJsonKeys.GAINED_CONTROL_OF).
                ToObject<EntityIDRecord>();
        }

        public override JObject ToJsonObject()
        {
            return new JObject
            {
                { MUSICJsonKeys.HEADER, MUSICHeader.ToJsonObject() },
                { MUSICJsonKeys.ORIGIN_ID, OriginID.ToJsonObject() },
                { MUSICJsonKeys.RECEIVER_ID, ReceiverID.ToJsonObject() },
                { MUSICJsonKeys.GAINED_CONTROL_OF, GainedControlOf.ToJsonObject() }
            };
        }

        public override void AcceptVisitor(IMUSICMessageVisitor visitor)
        {
            visitor.VisitControlGained(this);
        }

        public override object Clone()
        {
            ControlGainedMessage message = 
                new ControlGainedMessage
                (
                    MUSICHeader.ExerciseID,
                    (EntityIDRecord)OriginID.Clone(),
                    (EntityIDRecord)ReceiverID.Clone(),
                    (EntityIDRecord)GainedControlOf.Clone()
                );

            return message;
        }

        public override bool Equals(object obj)
        {
            if (obj is ControlGainedMessage)
            {
                ControlGainedMessage other = (ControlGainedMessage)obj;

                if (
                    base.Equals(other) &&
                    ReceiverID == other.ReceiverID &&
                    GainedControlOf == other.GainedControlOf
                   )
                    return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 272905447;
            hashCode = hashCode * -1521134210 + EqualityComparer<uint>.Default.GetHashCode(MUSICHeader.ExerciseID);
            hashCode = hashCode * -1521134210 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(OriginID);
            hashCode = hashCode * -1521134210 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(ReceiverID);
            hashCode = hashCode * -1521134210 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(GainedControlOf);
            return hashCode;
        }

        public static bool operator ==(ControlGainedMessage message1, ControlGainedMessage message2)
        {
            return EqualityComparer<ControlGainedMessage>.Default.Equals(message1, message2);
        }

        public static bool operator !=(ControlGainedMessage message1, ControlGainedMessage message2)
        {
            return !(message1 == message2);
        }
    }
}
