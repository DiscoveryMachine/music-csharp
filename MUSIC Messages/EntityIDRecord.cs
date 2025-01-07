/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System.Collections.Generic;
using MUSICLibrary.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenDis.Dis1998;

namespace MUSICLibrary.MUSIC_Messages
{
    public class EntityIDRecord : SiteAndAppID, IToJSON, IPrototype
    {
        public uint EntityID { get; private set; }

        [JsonConstructor]
        public EntityIDRecord(uint siteID, uint appID, uint entityID)
            : base(siteID, appID)
        {
            EntityID = entityID;
        }

        public static implicit operator EntityIDRecord(EntityID id) => new EntityIDRecord(id.Site, id.Application, id.Entity);

        public EntityIDRecord(SiteAndAppID siteAndApp, uint entityID) : base(siteAndApp.SiteID, siteAndApp.AppID)
        {
            EntityID = entityID;
        }

        public EntityIDRecord(JObject json)
        {
        }

        public new JObject ToJsonObject()
        {
            return new JObject
            {
                { MUSICJsonKeys.SITE_ID, SiteID },
                { MUSICJsonKeys.APP_ID, AppID },
                { MUSICJsonKeys.ENTITY_ID, EntityID }
            };
        }

        public SiteAndAppID GetSiteAndApp()
        {
            return new SiteAndAppID(SiteID, AppID);
        }

        public override bool Equals(object obj)
        {
            var record = obj as EntityIDRecord;
            return record != null &&
                   base.Equals(obj) &&
                   EntityID == record.EntityID;
        }

        public override int GetHashCode()
        {
            var hashCode = -697791091;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EntityID.GetHashCode();
            return hashCode;
        }

        public object Clone()
        {
            return new EntityIDRecord(SiteID, AppID, EntityID);
        }

        public static bool operator ==(EntityIDRecord record1, EntityIDRecord record2)
        {
            return EqualityComparer<EntityIDRecord>.Default.Equals(record1, record2);
        }

        public static bool operator !=(EntityIDRecord record1, EntityIDRecord record2)
        {
            return !(record1 == record2);
        }
    }
}
