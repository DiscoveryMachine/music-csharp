/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System.Collections.Generic;
using MUSICLibrary.Interfaces;
using Newtonsoft.Json.Linq;

namespace MUSICLibrary.MUSIC_Messages
{
    public class WaypointDataMessage : MUSICMessage
    {
        public ushort CurrentWaypointIndex { get; set; }
        public List<WaypointRecord> Waypoints { get; set; }

        public WaypointDataMessage() { }

        public WaypointDataMessage(uint exerciseID, EntityIDRecord entityIDRecord,
            ushort currentWaypointIndex, List<WaypointRecord> waypoints)
            : base (exerciseID, entityIDRecord)
        {
            CurrentWaypointIndex = currentWaypointIndex;
            Waypoints = waypoints;
        }

        public WaypointDataMessage(JObject jObject)
            : base (jObject[MUSICJsonKeys.HEADER].ToObject<MUSICHeader>(),
                  jObject[MUSICJsonKeys.ORIGIN_ID].ToObject<EntityIDRecord>())
        {
            CurrentWaypointIndex = jObject.GetValue(MUSICJsonKeys.CURRENT_WAYPOINT_INDEX).ToObject<ushort>();

            Waypoints = jObject.GetValue(MUSICJsonKeys.WAYPOINT_RECORDS).ToObject<List<WaypointRecord>>();
        }

        public override void AcceptVisitor(IMUSICMessageVisitor visitor)
        {
            visitor.VisitWaypointData(this);
        }

        public override object Clone()
        {
            List<WaypointRecord> clonedList = new List<WaypointRecord>();

            foreach (WaypointRecord waypointRecord in Waypoints)
                clonedList.Add((WaypointRecord)waypointRecord.Clone());

            WaypointDataMessage clonedMessage = new WaypointDataMessage(MUSICHeader.ExerciseID,
                (EntityIDRecord)OriginID.Clone(), CurrentWaypointIndex, clonedList);

            return clonedMessage;
        }

        public override JObject ToJsonObject()
        {
            JObject jobj = new JObject
            {
                { MUSICJsonKeys.HEADER, MUSICHeader.ToJsonObject() },
                { MUSICJsonKeys.ORIGIN_ID, OriginID.ToJsonObject() },
                { MUSICJsonKeys.CURRENT_WAYPOINT_INDEX, CurrentWaypointIndex },
                { MUSICJsonKeys.NUM_WAYPOINTS, Waypoints.Count }
            };

            JArray jArray = new JArray();
            Waypoints.ForEach(j => { jArray.Add(j.ToJsonObject()); });
            jobj.Add(MUSICJsonKeys.WAYPOINT_RECORDS, jArray);

            return jobj;
        }


        public override int GetHashCode()
        {
            var hashCode = -375801064;
            hashCode = hashCode * -1521134295 + EqualityComparer<uint>.Default.GetHashCode(MUSICHeader.ExerciseID);
            hashCode = hashCode * -1521134295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(OriginID);
            hashCode = hashCode * -1521134295 + EqualityComparer<ushort>.Default.GetHashCode(CurrentWaypointIndex);

            int sum = 0;
            foreach (var wp in Waypoints)
                sum += wp.GetHashCode();

            hashCode = hashCode * -1521134295 + sum;
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            var message = obj as WaypointDataMessage;

            if (Waypoints.Count != message.Waypoints.Count)
                return false;

            for (int i = 0; i < Waypoints.Count; i++)
            {
                if (!Waypoints[i].Equals(message.Waypoints[i]))
                    return false;
            }

            return message != null &&
                   base.Equals(obj) &&
                   CurrentWaypointIndex == message.CurrentWaypointIndex;
        }

        public static bool operator ==(WaypointDataMessage message1, WaypointDataMessage message2)
        {
            return EqualityComparer<WaypointDataMessage>.Default.Equals(message1, message2);
        }

        public static bool operator !=(WaypointDataMessage message1, WaypointDataMessage message2)
        {
            return !(message1 == message2);
        }
    }
}
