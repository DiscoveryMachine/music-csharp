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

using System.Collections.Generic;
using MUSICLibrary.Interfaces;
using Newtonsoft.Json.Linq;

namespace MUSICLibrary.MUSIC_Messages
{
    public class MUSICEventMessage : MUSICMessage
    {
        public string EventType { get; private set; }
        public JObject EventData { get; private set; }

        public MUSICEventMessage(uint exerciseID, EntityIDRecord originID,
            string eventType, JObject eventData) : base(exerciseID, originID)
        {
            EventType = eventType;
            EventData = eventData;
        }

        public MUSICEventMessage(JObject obj) :
            base(obj.GetValue(MUSICJsonKeys.HEADER).ToObject<MUSICHeader>(),
                obj.GetValue(MUSICJsonKeys.ORIGIN_ID).ToObject<EntityIDRecord>())
        {
            EventType = obj.GetValue(MUSICJsonKeys.EVENT_TYPE).ToObject<string>();
            EventData = obj.GetValue(MUSICJsonKeys.EVENT_DATA).ToObject<JObject>();
        }

        public override void AcceptVisitor(IMUSICMessageVisitor visitor)
        {
            visitor.VisitMUSICEvent(this);
        }

        public override object Clone()
        {
            return new MUSICEventMessage
                (
                    MUSICHeader.ExerciseID,
                    (EntityIDRecord)OriginID.Clone(),
                    string.Copy(EventType),
                    (JObject)EventData.DeepClone()
                );
        }

        public override JObject ToJsonObject()
        {
            return new JObject
            {
                { MUSICJsonKeys.HEADER, MUSICHeader.ToJsonObject() },
                { MUSICJsonKeys.ORIGIN_ID, OriginID.ToJsonObject() },
                { MUSICJsonKeys.EVENT_TYPE, EventType },
                { MUSICJsonKeys.EVENT_DATA, EventData }
            };
        }

        public override bool Equals(object obj)
        {
            if (obj is MUSICEventMessage)
            {
                var other = (MUSICEventMessage)obj;
                return base.Equals(other) && EventType == other.EventType && JObject.DeepEquals(EventData, other.EventData);
            }
            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 610186252;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EventType.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventData.ToString());
            return hashCode;
        }

        public static bool operator ==(MUSICEventMessage message1, MUSICEventMessage message2)
        {
            return EqualityComparer<MUSICEventMessage>.Default.Equals(message1, message2);
        }

        public static bool operator !=(MUSICEventMessage message1, MUSICEventMessage message2)
        {
            return !(message1 == message2);
        }
    }
}
