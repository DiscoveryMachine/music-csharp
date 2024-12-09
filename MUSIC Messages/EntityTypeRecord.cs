//=====================================================================
//DISCOVERY MACHINE, INC. PROPRIETARY INFORMATION
//
//This software is supplied under the terms of a license agreement
//or nondisclosure agreement with Discovery Machine, Inc. and may
//not be copied or disclosed except in accordance with the terms  of
//that agreement.
//
//Copyright 2022 Discovery Machine, Inc. All Rights Reserved.
//=====================================================================

using MUSICLibrary.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenDis.Dis1998;
using System.Collections.Generic;

namespace MUSICLibrary.MUSIC_Messages
{
    public class EntityTypeRecord : IToJSON, IPrototype
    {
        [JsonProperty("kind")]
        public EntityKind Kind { get; set; }

        [JsonProperty("domain")]
        public EntityDomain Domain { get; set; }

        [JsonProperty("country")]
        public EntityCountry Country { get; set; }

        [JsonProperty("category")]
        public byte Category { get; set; }

        [JsonProperty("subcategory")]
        public byte Subcategory { get; set; }

        [JsonProperty("specific")]
        public byte Specific { get;  set; }

        [JsonProperty("extra")]
        public byte Extra { get; set; }

        public EntityTypeRecord() { }

        public EntityTypeRecord(EntityKind kind, EntityDomain domain,
            EntityCountry country, byte category, byte subcategory,
            byte specificInfo, byte extraValue)
        {
            Kind = kind;
            Domain = domain;
            Country = country;
            Category = category;
            Subcategory = subcategory;
            Specific = specificInfo;
            Extra = extraValue;
        }

        public EntityTypeRecord(JObject json)
        {
            if (json is null)
                return;

            Kind = json.GetValue("kind").ToObject<EntityKind>();
            Domain = json.GetValue("domain").ToObject<EntityDomain>();
            Country = json.GetValue("country").ToObject<EntityCountry>();
            Category = (byte) json.GetValue("category");
            Subcategory = (byte) json.GetValue("subcategory");
            Specific = (byte) json.GetValue("specific");
            Extra = (byte) json.GetValue("extra");
        }

        public EntityTypeRecord(EntityType entityType)
        {
            Kind = (EntityKind)entityType.EntityKind;
            Domain = (EntityDomain)entityType.Domain;
            Country = (EntityCountry)entityType.Country;
            Category = entityType.Category;
            Subcategory = entityType.Subcategory;
            Specific = entityType.Specific;
            Extra = entityType.Extra;
        }

        public JObject ToJsonObject()
        {
            JObject obj = new JObject();

            obj.Add("kind", (int) Kind);
            obj.Add("domain", (int) Domain);
            obj.Add("country", (int) Country);
            obj.Add("category", Category);
            obj.Add("subcategory", Subcategory);
            obj.Add("specific", Specific);
            obj.Add("extra", Extra);

            return obj;
        }

        public override bool Equals(object obj)
        {
            if(obj is EntityTypeRecord)
            {
                EntityTypeRecord other = (EntityTypeRecord) obj;
                if(Kind == other.Kind && Domain == other.Domain &&
                    Country == other.Country && Category == other.Category &&
                    Subcategory == other.Subcategory && Specific ==
                    other.Specific && Extra == other.Extra)
                {
                    return true;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 965029686;
            hashCode = hashCode * -1521134295 + Kind.GetHashCode();
            hashCode = hashCode * -1521134295 + Domain.GetHashCode();
            hashCode = hashCode * -1521134295 + Country.GetHashCode();
            hashCode = hashCode * -1521134295 + Category.GetHashCode();
            hashCode = hashCode * -1521134295 + Subcategory.GetHashCode();
            hashCode = hashCode * -1521134295 + Specific.GetHashCode();
            hashCode = hashCode * -1521134295 + Extra.GetHashCode();
            return hashCode;
        }

        public object Clone()
        {
            return new EntityTypeRecord(Kind, Domain, Country, Category, Subcategory, Specific, Extra);
        }

        public static bool operator ==(EntityTypeRecord message1, EntityTypeRecord message2)
        {
            return EqualityComparer<EntityTypeRecord>.Default.Equals(message1, message2);
        }

        public static bool operator !=(EntityTypeRecord message1, EntityTypeRecord message2)
        {
            return !(message1 == message2);
        }
    }
}
