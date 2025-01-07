/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System.Collections.Generic;
using MUSICLibrary.Interfaces;
using MUSICLibrary.MUSIC_Messages.Construct_Data.Dead_Reckoning;
using Newtonsoft.Json.Linq;

namespace MUSICLibrary.MUSIC_Messages
{
    public class PhysicalRecord : IToJSON, IPrototype
    {
        private const string FORCE = "force";
        private const string ENTITY_TYPE = "entityType";
        private const string LOCATION = "location";
        private const string ORIENTATION = "orientation";
        private const string LINEAR_VELOCITY = "velocity";
        private const string DEAD_RECKONING = "deadReck";
        private const string DAMAGE = "damageRecord";

        public DeadReckoningParametersRecord DeadReckoningParameters { get; set; }
        public MUSICVector3 Location { get; set; }
        public DamageRecord Damage { get; set; }
        public EntityTypeRecord EntityType { get; set; }
        public Force ForceIDField { get; set; }
        public MUSICVector3 LinearVelocity { get; set; }
        public MUSICVector3 Orientation { get; set; }

        public PhysicalRecord()
        {
        }

        public PhysicalRecord(JObject json)
        {
            DeadReckoningParameters = new DeadReckoningParametersRecord(json[DEAD_RECKONING] as JObject);
            Location = new MUSICVector3(json[LOCATION] as JObject);
            Orientation = new MUSICVector3(json[ORIENTATION] as JObject);
            Damage = new DamageRecord(json[DAMAGE] as JObject);
            EntityType = new EntityTypeRecord(json[ENTITY_TYPE] as JObject);
            ForceIDField = json.ContainsKey(FORCE) ? (Force)(int)json[FORCE] : Force.Other;
            LinearVelocity = new MUSICVector3(json[LINEAR_VELOCITY] as JObject);
        }

        public void Update(PhysicalRecord updatedPhysical)
        {
            if (updatedPhysical is null) return;

            DeadReckoningParameters = updatedPhysical.DeadReckoningParameters is null ? DeadReckoningParameters : updatedPhysical.DeadReckoningParameters;
            Location = updatedPhysical.Location is null ? Location : updatedPhysical.Location;
            Damage = updatedPhysical.Damage is null ? Damage : updatedPhysical.Damage;
            EntityType = updatedPhysical.EntityType is null ? EntityType : updatedPhysical.EntityType;
            ForceIDField = updatedPhysical.ForceIDField;
            LinearVelocity = updatedPhysical.LinearVelocity is null ? LinearVelocity : updatedPhysical.LinearVelocity;
            Orientation = updatedPhysical.Orientation is null ? Orientation : updatedPhysical.Orientation;
        }

        public JObject ToJsonObject()
        {
            JObject json = new JObject();

            json[FORCE] = (int)ForceIDField;
            json[ENTITY_TYPE] = EntityType?.ToJsonObject();
            json[LOCATION] = Location?.ToJsonObject();
            json[ORIENTATION] = Orientation?.ToJsonObject();
            json[LINEAR_VELOCITY] = LinearVelocity?.ToJsonObject();
            json[DEAD_RECKONING] = DeadReckoningParameters?.ToJsonObject();
            json[DAMAGE] = Damage?.ToJsonObject();

            return json;
        }

        public override bool Equals(object obj)
        {
            var record = obj as PhysicalRecord;
            return record != null &&
                   EqualityComparer<DeadReckoningParametersRecord>.Default.Equals(DeadReckoningParameters, record.DeadReckoningParameters) &&
                   EqualityComparer<MUSICVector3>.Default.Equals(Location, record.Location) &&
                   EqualityComparer<DamageRecord>.Default.Equals(Damage, record.Damage) &&
                   EqualityComparer<EntityTypeRecord>.Default.Equals(EntityType, record.EntityType) &&
                   ForceIDField == record.ForceIDField &&
                   EqualityComparer<MUSICVector3>.Default.Equals(LinearVelocity, record.LinearVelocity) &&
                   EqualityComparer<MUSICVector3>.Default.Equals(Orientation, record.Orientation);
        }

        public override int GetHashCode()
        {
            var hashCode = 852559290;
            hashCode = hashCode * -1521134295 + EqualityComparer<DeadReckoningParametersRecord>.Default.GetHashCode(DeadReckoningParameters);
            hashCode = hashCode * -1521134295 + EqualityComparer<MUSICVector3>.Default.GetHashCode(Location);
            hashCode = hashCode * -1521134295 + EqualityComparer<DamageRecord>.Default.GetHashCode(Damage);
            hashCode = hashCode * -1521134295 + EqualityComparer<EntityTypeRecord>.Default.GetHashCode(EntityType);
            hashCode = hashCode * -1521134295 + ForceIDField.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<MUSICVector3>.Default.GetHashCode(LinearVelocity);
            hashCode = hashCode * -1521134295 + EqualityComparer<MUSICVector3>.Default.GetHashCode(Orientation);
            return hashCode;
        }

        public object Clone()
        {
            PhysicalRecord clone = new PhysicalRecord();

            clone.Damage = (DamageRecord)Damage?.Clone();
            clone.EntityType = (EntityTypeRecord)EntityType?.Clone();
            clone.ForceIDField = ForceIDField;
            clone.LinearVelocity = (MUSICVector3)LinearVelocity?.Clone();
            clone.Location = (MUSICVector3)Location?.Clone();
            clone.Orientation = (MUSICVector3)Orientation?.Clone();
            clone.DeadReckoningParameters = (DeadReckoningParametersRecord)DeadReckoningParameters?.Clone();

            return clone;
        }

        public static bool operator ==(PhysicalRecord record1, PhysicalRecord record2)
        {
            return EqualityComparer<PhysicalRecord>.Default.Equals(record1, record2);
        }

        public static bool operator !=(PhysicalRecord record1, PhysicalRecord record2)
        {
            return !(record1 == record2);
        }
    }
}
