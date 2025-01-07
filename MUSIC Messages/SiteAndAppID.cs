/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using MUSICLibrary.Interfaces;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MUSICLibrary.MUSIC_Messages
{
    public class SiteAndAppID : IToJSON
    {
        public SiteAndAppID() { }

        public SiteAndAppID(uint siteID, uint appID)
        {
            SiteID = siteID;
            AppID = appID;
        }

        public uint SiteID { get; set; }
        public uint AppID { get; set; }

        public override bool Equals(object obj)
        {
            var iD = obj as SiteAndAppID;
            return iD != null &&
                   SiteID == iD.SiteID &&
                   AppID == iD.AppID;
        }

        public override int GetHashCode()
        {
            var hashCode = -421393812;
            hashCode = hashCode * -1521134295 + SiteID.GetHashCode();
            hashCode = hashCode * -1521134295 + AppID.GetHashCode();
            return hashCode;
        }

        /**Converts the object into a Json Object.
        * @return JObject : the converted object's parameters*/
        public JObject ToJsonObject()
        {
            JObject obj = new JObject();
            
            obj.Add("siteID", SiteID);
            obj.Add("appID", AppID);

            return obj;
        }

        public static bool operator ==(SiteAndAppID iD1, SiteAndAppID iD2)
        {
            return EqualityComparer<SiteAndAppID>.Default.Equals(iD1, iD2);
        }

        public static bool operator !=(SiteAndAppID iD1, SiteAndAppID iD2)
        {
            return !(iD1 == iD2);
        }
    }
}
