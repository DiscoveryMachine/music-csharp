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
