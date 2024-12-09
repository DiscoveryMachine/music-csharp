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

using System.Collections.Generic;
using MUSICLibrary.Interfaces;
using Newtonsoft.Json.Linq;

namespace MUSICLibrary.MUSIC_Messages.MUSIC_Request_Messages
{
    public class ParameterizeConstructRequestMessage : MUSICRequestMessage, IMUSICCommandMessage
    {
        public EntityIDRecord GhostedID { get; set; }
        public JObject ConstructParameters { get; set; }
        public uint CommandIdentifier => 454013004;

        public ParameterizeConstructRequestMessage(uint exerciseID, EntityIDRecord originID,
            EntityIDRecord receiverID, uint requestID, JObject constructParameters, EntityIDRecord ghostedID = null)
            : base(exerciseID, originID, receiverID, requestID)
        {
            GhostedID = ghostedID;
            ConstructParameters = constructParameters;
        }

        public ParameterizeConstructRequestMessage(JObject jobj) : base(jobj)
        {
            if (jobj.TryGetValue(MUSICJsonKeys.GHOSTED_ID, out JToken ghostIdToken))
                GhostedID = jobj[MUSICJsonKeys.GHOSTED_ID].ToObject<EntityIDRecord>();

            ConstructParameters = jobj[MUSICJsonKeys.CONSTRUCT_PARAMETERS].ToObject<JObject>();
        }

        public override void AcceptVisitor(IMUSICMessageVisitor visitor)
        {
            visitor.VisitParameterizeConstructRequest(this);
        }

        public override object Clone()
        {
            return new ParameterizeConstructRequestMessage
                (
                    MUSICHeader.ExerciseID,
                    (EntityIDRecord)OriginID.Clone(),
                    (EntityIDRecord)ReceiverID.Clone(),
                    RequestID,
                    (JObject)ConstructParameters.DeepClone(),
                    (EntityIDRecord)GhostedID?.Clone()
                );
        }

        public override JObject ToJsonObject()
        {
            JObject jobj = new JObject
            {
                { MUSICJsonKeys.HEADER, MUSICHeader.ToJsonObject() },
                { MUSICJsonKeys.COMMAND_IDENTIFIER, CommandIdentifier },
                { MUSICJsonKeys.ORIGIN_ID, OriginID.ToJsonObject() },
                { MUSICJsonKeys.RECEIVER_ID, ReceiverID.ToJsonObject() },
                { MUSICJsonKeys.REQUEST_ID, RequestID },
                { MUSICJsonKeys.CONSTRUCT_PARAMETERS, ConstructParameters }
            };

            if (GhostedID != null )
                jobj.Add(MUSICJsonKeys.GHOSTED_ID, GhostedID.ToJsonObject());

            return jobj;
        }

        public override bool Equals(object obj)
        {
            if (obj is ParameterizeConstructRequestMessage)
            {
                ParameterizeConstructRequestMessage other = (ParameterizeConstructRequestMessage)obj;

                if (
                    base.Equals(other) &&
                    JObject.DeepEquals(ConstructParameters, other.ConstructParameters) &&
                    GhostedID == other.GhostedID
                    )
                    return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = -158961684;

            hashCode = hashCode * -1521130295 + EqualityComparer<uint>.Default.GetHashCode(MUSICHeader.ExerciseID);
            hashCode = hashCode * -1521130295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(OriginID);
            hashCode = hashCode * -1521130295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(ReceiverID);
            hashCode = hashCode * -1521130295 + EqualityComparer<uint>.Default.GetHashCode(RequestID);
            hashCode = hashCode * -1521130295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(GhostedID);
            hashCode = hashCode * -1521130295 + EqualityComparer<string>.Default.GetHashCode(ConstructParameters.ToString());

            return hashCode;
        }

        public static bool operator ==(ParameterizeConstructRequestMessage message1, ParameterizeConstructRequestMessage message2)
        {
            return EqualityComparer<ParameterizeConstructRequestMessage>.Default.Equals(message1, message2);
        }

        public static bool operator !=(ParameterizeConstructRequestMessage message1, ParameterizeConstructRequestMessage message2)
        {
            return !(message1 == message2);
        }
    }
}
