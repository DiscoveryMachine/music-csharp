/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System;
using System.Collections.Generic;
using MUSICLibrary.Interfaces;
using Newtonsoft.Json.Linq;

namespace MUSICLibrary.MUSIC_Messages.MUSIC_Request_Messages
{
    public class CreateConstructRequestMessage : MUSICRequestMessage, IMUSICCommandMessage
    {
        public string ConstructType { get; private set; }
        public string ConstructCallsign { get; private set; }
        public MUSICVector3 ConstructLocation { get; private set; }
        public MUSICVector3 ConstructOrientation { get; private set; }
        public uint CommandIdentifier => 454013002;

        public CreateConstructRequestMessage(uint exerciseID, EntityIDRecord originID,
            EntityIDRecord receiverID, uint requestID, string constructType,
            string constructCallsign, MUSICVector3 constructLocation, MUSICVector3 constructOrientation)
            : base(exerciseID, originID, receiverID, requestID)
        {
            ConstructType = constructType;
            ConstructCallsign = constructCallsign;
            ConstructLocation = constructLocation;
            ConstructOrientation = constructOrientation;
        }

        public CreateConstructRequestMessage(JObject jobj) : base(jobj)
        {
            ConstructType = jobj[MUSICJsonKeys.CONSTRUCT_TYPE].ToObject<string>();
            ConstructCallsign = jobj[MUSICJsonKeys.CONSTRUCT_CALLSIGN].ToObject<string>();
            ConstructLocation = jobj[MUSICJsonKeys.CONSTRUCT_LOCATION].ToObject<MUSICVector3>();
            ConstructOrientation = jobj[MUSICJsonKeys.CONSTRUCT_ORIENTATION].ToObject<MUSICVector3>();
        }

        public override void AcceptVisitor(IMUSICMessageVisitor visitor)
        {
            visitor.VisitCreateConstructRequest(this);
        }

        public override object Clone()
        {
            return new CreateConstructRequestMessage
                (
                    MUSICHeader.ExerciseID,
                    (EntityIDRecord)OriginID.Clone(),
                    (EntityIDRecord)ReceiverID.Clone(),
                    RequestID,
                    string.Copy(ConstructType),
                    string.Copy(ConstructCallsign),
                    (MUSICVector3)ConstructLocation.Clone(),
                    (MUSICVector3)ConstructOrientation.Clone()
                );
        }

        public override JObject ToJsonObject()
        {
            return new JObject
            {
                { MUSICJsonKeys.HEADER, MUSICHeader.ToJsonObject() },
                { MUSICJsonKeys.COMMAND_IDENTIFIER, CommandIdentifier },
                { MUSICJsonKeys.ORIGIN_ID, OriginID.ToJsonObject() },
                { MUSICJsonKeys.RECEIVER_ID, ReceiverID.ToJsonObject() },
                { MUSICJsonKeys.REQUEST_ID, RequestID },
                { MUSICJsonKeys.CONSTRUCT_TYPE, ConstructType },
                { MUSICJsonKeys.CONSTRUCT_CALLSIGN, ConstructCallsign },
                { MUSICJsonKeys.CONSTRUCT_LOCATION, ConstructLocation.ToJsonObject() },
                { MUSICJsonKeys.CONSTRUCT_ORIENTATION, ConstructOrientation.ToJsonObject() }
            };
        }

        public override bool Equals(object obj)
        {
            if (obj is CreateConstructRequestMessage)
            {
                CreateConstructRequestMessage other = (CreateConstructRequestMessage)obj;

                if (base.Equals(other) &&
                    ConstructType.Equals(other.ConstructType) &&
                    ConstructCallsign.Equals(other.ConstructCallsign) &&
                    ConstructLocation.Equals(other.ConstructLocation) &&
                    ConstructOrientation.Equals(other.ConstructOrientation)
                    )
                    return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 510219160;
            hashCode = hashCode * -1521134295 + EqualityComparer<uint>.Default.GetHashCode(MUSICHeader.ExerciseID);
            hashCode = hashCode * -1521134295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(OriginID);
            hashCode = hashCode * -1521134295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(ReceiverID);
            hashCode = hashCode * -1521134295 + EqualityComparer<uint>.Default.GetHashCode(RequestID);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ConstructType);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ConstructCallsign);
            hashCode = hashCode * -1521134295 + EqualityComparer<MUSICVector3>.Default.GetHashCode(ConstructLocation);
            hashCode = hashCode * -1521134295 + EqualityComparer<MUSICVector3>.Default.GetHashCode(ConstructOrientation);
            return hashCode;
        }

        public static bool operator ==(CreateConstructRequestMessage message1, CreateConstructRequestMessage message2)
        {
            return EqualityComparer<CreateConstructRequestMessage>.Default.Equals(message1, message2);
        }

        public static bool operator !=(CreateConstructRequestMessage message1, CreateConstructRequestMessage message2)
        {
            return !(message1 == message2);
        }
    }
}
