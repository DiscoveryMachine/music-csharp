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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MUSICLibrary.MUSIC_Messages
{
    public class WaypointRecord : IToJSON, IPrototype
    {
        [JsonProperty("worldCoordinateRecord")]
        public MUSICVector3 WorldCoordinateRecord { get; private set; }
        [JsonProperty("estimatedArrivalTime")]
        public uint EstimatedArrivalTime { get; private set; }
        [JsonProperty("arrivalTimeError")]
        public uint ArrivalTimeError { get; private set; }

        // Constants for Key lookups.
        private const string WORLD_COORDINATE_RECORD = "worldCoordinateRecord";
        private const string ESTIMATED_ARRIVAL_TIME = "estimatedArrivalTime";
        private const string ARRIVAL_TIME_ERROR = "arrivalTimeError";

        public WaypointRecord() { }

        public WaypointRecord (MUSICVector3 worldCoordinateRecord, uint estimatedArrivalTime,
            uint arrivalTimeError)
        {
            WorldCoordinateRecord = worldCoordinateRecord;
            EstimatedArrivalTime = estimatedArrivalTime;
            ArrivalTimeError = arrivalTimeError;
        }

        public WaypointRecord(JObject jobj)
        {
            WorldCoordinateRecord = jobj[WORLD_COORDINATE_RECORD].ToObject<MUSICVector3>();
            EstimatedArrivalTime = jobj[ESTIMATED_ARRIVAL_TIME].ToObject<uint>();
            ArrivalTimeError = jobj[ARRIVAL_TIME_ERROR].ToObject<uint>();
        }

        public JObject ToJsonObject()
        {
            JObject jobj = new JObject();

            jobj.Add(WORLD_COORDINATE_RECORD, WorldCoordinateRecord.ToJsonObject());
            jobj.Add(ESTIMATED_ARRIVAL_TIME, EstimatedArrivalTime);
            jobj.Add(ARRIVAL_TIME_ERROR, ArrivalTimeError);

            return jobj;
        }

        public override bool Equals(object obj)
        {
            var record = obj as WaypointRecord;
            return record != null &&
                   EqualityComparer<MUSICVector3>.Default.Equals(WorldCoordinateRecord, record.WorldCoordinateRecord) &&
                   EstimatedArrivalTime == record.EstimatedArrivalTime &&
                   ArrivalTimeError == record.ArrivalTimeError;
        }

        public override int GetHashCode()
        {
            var hashCode = 400749366;
            hashCode = hashCode * -1521134295 + EqualityComparer<MUSICVector3>.Default.GetHashCode(WorldCoordinateRecord);
            hashCode = hashCode * -1521134295 + EstimatedArrivalTime.GetHashCode();
            hashCode = hashCode * -1521134295 + ArrivalTimeError.GetHashCode();
            return hashCode;
        }

        public object Clone()
        {
            return new WaypointRecord((MUSICVector3)WorldCoordinateRecord.Clone(), EstimatedArrivalTime, ArrivalTimeError);
        }

        public static bool operator ==(WaypointRecord record1, WaypointRecord record2)
        {
            return EqualityComparer<WaypointRecord>.Default.Equals(record1, record2);
        }

        public static bool operator !=(WaypointRecord record1, WaypointRecord record2)
        {
            return !(record1 == record2);
        }
    }
}
