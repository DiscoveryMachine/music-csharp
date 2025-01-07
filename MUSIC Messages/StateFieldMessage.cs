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
    public class StateFieldMessage : MUSICMessage
    {
        public JObject StateDataObject { get; set; }

        public StateFieldMessage() { }

        public StateFieldMessage(uint exerciseID, EntityIDRecord originID, JObject jObject)
            : base (exerciseID, originID)
        {
            StateDataObject = jObject;
        }

        public StateFieldMessage(JObject jObject)
            : base (jObject[MUSICJsonKeys.HEADER].ToObject<MUSICHeader>(),
                  jObject[MUSICJsonKeys.ORIGIN_ID].ToObject<EntityIDRecord>())
        {
            StateDataObject = jObject.GetValue(MUSICJsonKeys.STATE_DATA).ToObject<JObject>();
        }

        public override JObject ToJsonObject()
        {
            return new JObject
            {
                { MUSICJsonKeys.HEADER, MUSICHeader.ToJsonObject() },
                { MUSICJsonKeys.ORIGIN_ID, OriginID.ToJsonObject() },
                { MUSICJsonKeys.STATE_DATA, StateDataObject }
            };
        }

        public override void AcceptVisitor(IMUSICMessageVisitor visitor)
        {
            visitor.VisitStateField(this);
        }

        public override object Clone()
        {
            EntityIDRecord originClone = new EntityIDRecord(OriginID.SiteID, OriginID.AppID, OriginID.EntityID);
            JObject stateDataObjectClone = (JObject)StateDataObject.DeepClone();

            StateFieldMessage message = 
                new StateFieldMessage(MUSICHeader.ExerciseID, originClone, stateDataObjectClone);

            return message;
        }

        public override bool Equals(object obj)
        {
            if (obj is StateFieldMessage)
            {
                StateFieldMessage other = (StateFieldMessage)obj;

                if (
                    base.Equals(other) &&
                    JObject.DeepEquals(StateDataObject, other.StateDataObject)
                   )
                    return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return 1566961789 + EqualityComparer<uint>.Default.GetHashCode(MUSICHeader.ExerciseID)
                + EqualityComparer<EntityIDRecord>.Default.GetHashCode(OriginID)
                + EqualityComparer<string>.Default.GetHashCode(StateDataObject.ToString());
        }

        public static bool operator ==(StateFieldMessage message1, StateFieldMessage message2)
        {
            return EqualityComparer<StateFieldMessage>.Default.Equals(message1, message2);
        }

        public static bool operator !=(StateFieldMessage message1, StateFieldMessage message2)
        {
            return !(message1 == message2);
        }
    }
}
