/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using MUSICLibrary.Interfaces;
using MUSICLibrary.MUSIC_Messages.PerceptionData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MUSICLibrary.MUSIC_Messages
{
    public class PerceptionDataMessage : MUSICMessage
    {
        [JsonProperty(MUSICJsonKeys.PERCEPTION_RECORDS)]
        public List<PerceptionRecord> PerceptionRecords { get; private set; }

        // This stops a Newtonsoft exception from being thrown.
        public PerceptionDataMessage() { }

        public PerceptionDataMessage(uint exerciseID, EntityIDRecord originID, List<PerceptionRecord> perceptionRecords)
            : base(exerciseID, originID)
        {
            PerceptionRecords = perceptionRecords;
        }

        public PerceptionDataMessage(JObject jobj)
            : base( jobj[MUSICJsonKeys.HEADER].ToObject<MUSICHeader>(), jobj[MUSICJsonKeys.ORIGIN_ID].ToObject<EntityIDRecord>())
        {
            PerceptionRecords = jobj[MUSICJsonKeys.PERCEPTION_RECORDS].ToObject<List<PerceptionRecord>>();
        }

        public override void AcceptVisitor(IMUSICMessageVisitor visitor)
        {
            visitor.VisitPerceptionData(this);
        }

        public override object Clone()
        {
            List<PerceptionRecord> clonedList = new List<PerceptionRecord>();

            foreach(PerceptionRecord perceptionRecord in PerceptionRecords)
                clonedList.Add((PerceptionRecord)perceptionRecord.Clone());

            return new PerceptionDataMessage(MUSICHeader.ExerciseID, (EntityIDRecord)OriginID.Clone(), clonedList);
        }

        public override JObject ToJsonObject()
        {
            JObject jobj = new JObject
            {
                { MUSICJsonKeys.HEADER, MUSICHeader.ToJsonObject() },
                { MUSICJsonKeys.ORIGIN_ID, OriginID.ToJsonObject() },
                { MUSICJsonKeys.NUM_PERCEPTIONS, PerceptionRecords.Count }
            };

            JArray jArray = new JArray();
            PerceptionRecords.ForEach(i => { jArray.Add(i.ToJsonObject()); });
            jobj.Add(MUSICJsonKeys.PERCEPTION_RECORDS, jArray);

            return jobj;
        }

        public override bool Equals(object obj)
        {
            if (obj is PerceptionDataMessage)
            {
                PerceptionDataMessage other = (PerceptionDataMessage)obj;

                if (base.Equals(other) &&
                    Enumerable.Range(0, 1).All(i => PerceptionRecords.Count == other.PerceptionRecords.Count) &&
                    PerceptionRecords.OfType<WaypointRecord>().SequenceEqual(other.PerceptionRecords.OfType<WaypointRecord>())
                    )
                    return true;
            }

            return false;
        }

        public bool Equals(PerceptionDataMessage other)
        {
            return other != null &&
                   base.Equals(other) &&
                   EqualityComparer<List<PerceptionRecord>>.Default.Equals(PerceptionRecords, other.PerceptionRecords);
        }

        public override int GetHashCode()
        {
            var hashCode = -1573912470;
            hashCode = hashCode * -1521134295 + base.GetHashCode();

            foreach(PerceptionRecord record in PerceptionRecords)
            {
                hashCode = hashCode * -1521134295 + record.GetHashCode();
            }
            
            return hashCode;
        }

        public static bool operator ==(PerceptionDataMessage message1, PerceptionDataMessage message2)
        {
            return EqualityComparer<PerceptionDataMessage>.Default.Equals(message1, message2);
        }

        public static bool operator !=(PerceptionDataMessage message1, PerceptionDataMessage message2)
        {
            return !(message1 == message2);
        }
    }
}
