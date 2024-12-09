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

namespace MUSICLibrary.MUSIC_Messages.PerceptionData
{
    public class PerceptionRecord : IToJSON, IPrototype
    {
        [JsonProperty("perceptionID")]
        public EntityIDRecord PerceptionID { get; private set; }

        [JsonProperty("force")]
        public Force ForceIDField { get; private set; }

        [JsonProperty("entityType")]
        public EntityTypeRecord EntityTypeRecord { get; private set; }

        [JsonProperty("system")]
        public SensorType PerceptionSystem { get; private set; }

        [JsonProperty("accuracy")]
        public float Accuracy { get; private set; }

        [JsonProperty("perceptionErrors")]
        public PerceptionErrors PerceptionErrors { get; private set; }

        private const string PERCEPTION_ID = "perceptionID";
        private const string FORCE_ID_FIELD = "force";
        private const string ENTITY_TYPE_RECORD = "entityType";
        private const string PERCEPTION_SYSTEM = "system";
        private const string ACCURACY = "accuracy";
        private const string PERCEPTION_ERRORS = "perceptionErrors";

        public PerceptionRecord() { }

        public PerceptionRecord(EntityIDRecord perceptionID, Force forceIDField,
            EntityTypeRecord entityTypeRecord, SensorType perceptionSystem, float accuracy,
            PerceptionErrors perceptionErrors)
        {
            PerceptionID = perceptionID;
            ForceIDField = forceIDField;
            EntityTypeRecord = entityTypeRecord;
            PerceptionSystem = perceptionSystem;
            Accuracy = accuracy;
            PerceptionErrors = perceptionErrors;
        }

        public PerceptionRecord(JObject jobj)
        {
            PerceptionID = jobj[PERCEPTION_ID].ToObject<EntityIDRecord>();
            ForceIDField = jobj[FORCE_ID_FIELD].ToObject<Force>();
            EntityTypeRecord = jobj[ENTITY_TYPE_RECORD].ToObject<EntityTypeRecord>();
            PerceptionSystem = jobj[PERCEPTION_SYSTEM].ToObject<SensorType>();
            Accuracy = jobj[ACCURACY].ToObject<float>();
            PerceptionErrors = jobj[PERCEPTION_ERRORS].ToObject<PerceptionErrors>();
        }

        public JObject ToJsonObject()
        {
            JObject jobj = new JObject();
            jobj.Add(PERCEPTION_ID, PerceptionID.ToJsonObject());
            jobj.Add(FORCE_ID_FIELD, (int)ForceIDField);
            jobj.Add(ENTITY_TYPE_RECORD, EntityTypeRecord.ToJsonObject());
            jobj.Add(PERCEPTION_SYSTEM, (int)PerceptionSystem);
            jobj.Add(ACCURACY, Accuracy);
            jobj.Add(PERCEPTION_ERRORS, PerceptionErrors.ToJsonObject());
            return jobj;
        }

        public override bool Equals(object obj)
        {
            if (obj is PerceptionRecord)
            {
                PerceptionRecord other = (PerceptionRecord) obj;
                return PerceptionErrors.Equals(other.PerceptionErrors) &&
                    PerceptionID.Equals(other.PerceptionID) &&
                    PerceptionSystem.Equals(other.PerceptionSystem) &&
                    EntityTypeRecord.Equals(other.EntityTypeRecord) &&
                    ForceIDField.Equals(other.ForceIDField) &&
                    Accuracy.Equals(other.Accuracy);
            }
            else return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 852925;
            hashCode = hashCode * -1521134295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(PerceptionID);
            hashCode = hashCode * -1521134295 + ForceIDField.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<EntityTypeRecord>.Default.GetHashCode(EntityTypeRecord);
            hashCode = hashCode * -1521134295 + PerceptionSystem.GetHashCode();
            hashCode = hashCode * -1521134295 + Accuracy.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<PerceptionErrors>.Default.GetHashCode(PerceptionErrors);
            return hashCode;
        }

        public object Clone()
        {
            return new PerceptionRecord(
                (EntityIDRecord)PerceptionID.Clone(),
                ForceIDField,
                (EntityTypeRecord)EntityTypeRecord.Clone(),
                PerceptionSystem,
                Accuracy,
                (PerceptionErrors)PerceptionErrors.Clone()
            );
        }

        public static bool operator ==(PerceptionRecord pr1, PerceptionRecord pr2)
        {
            return EqualityComparer<PerceptionRecord>.Default.Equals(pr1, pr2);
        }

        public static bool operator !=(PerceptionRecord pr1, PerceptionRecord pr2)
        {
            return !(pr1 == pr2);
        }
    }
}
