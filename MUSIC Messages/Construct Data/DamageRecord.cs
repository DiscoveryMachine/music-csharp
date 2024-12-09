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
    public class DamageRecord : IToJSON, IPrototype
    {
        private const string CATASTROPHIC_DAMAGE = "catastrophicDamage";
        private const string MOBILITY_DAMAGE = "mobilityDamage";
        private const string FIREPOWER_DISABLED = "firepowerDisabled";

        public double CatastrophicDamage { get; set; }
        public double MobilityDamage { get; set; }
        public bool FirepowerDisabled { get; set; }

        public DamageRecord(JObject json)
        {
            if (json is null)
                return;

            CatastrophicDamage = (double)json[CATASTROPHIC_DAMAGE];
            MobilityDamage = (double)json[MOBILITY_DAMAGE];
            FirepowerDisabled = (bool)json[FIREPOWER_DISABLED];
        }

        public DamageRecord(double catastrophicDamage, double mobilityDamage, bool firepowerDisabled)
        {
            CatastrophicDamage = catastrophicDamage;
            MobilityDamage = mobilityDamage;
            FirepowerDisabled = firepowerDisabled;
        }

        public DamageRecord()
        {
        }

        public JObject ToJsonObject()
        {
            JObject json = new JObject();

            json[CATASTROPHIC_DAMAGE] = CatastrophicDamage;
            json[MOBILITY_DAMAGE] = MobilityDamage;
            json[FIREPOWER_DISABLED] = FirepowerDisabled;

            return json;
        }

        public override bool Equals(object obj)
        {
            var record = obj as DamageRecord;
            return record != null &&
                   CatastrophicDamage == record.CatastrophicDamage &&
                   MobilityDamage == record.MobilityDamage &&
                   FirepowerDisabled == record.FirepowerDisabled;
        }

        public override int GetHashCode()
        {
            var hashCode = 134427542;
            hashCode = hashCode * -1521134295 + CatastrophicDamage.GetHashCode();
            hashCode = hashCode * -1521134295 + MobilityDamage.GetHashCode();
            hashCode = hashCode * -1521134295 + FirepowerDisabled.GetHashCode();
            return hashCode;
        }

        public object Clone()
        {
            return new DamageRecord(CatastrophicDamage, MobilityDamage, FirepowerDisabled);
        }

        public static bool operator ==(DamageRecord record1, DamageRecord record2)
        {
            return EqualityComparer<DamageRecord>.Default.Equals(record1, record2);
        }

        public static bool operator !=(DamageRecord record1, DamageRecord record2)
        {
            return !(record1 == record2);
        }
    }
}
