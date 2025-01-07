/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using MUSICLibrary.Interfaces;
using MUSICLibrary.MUSIC_Messages.MUSIC_Request_Messages;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MUSICLibrary.MUSIC_Messages.MUSIC_Response_Messages
{
    public class CreateConstructResponseMessage : MUSICResponseMessage, IMUSICCommandMessage
    {
        public EntityIDRecord ConstructID { get; set; }
        public uint CommandIdentifier => 454013003;

        protected CreateConstructResponseMessage(uint exerciseID, EntityIDRecord originID,
            EntityIDRecord receiverID, uint requestID, EntityIDRecord constructID, RequestStatus status)
            : base(exerciseID, originID, receiverID, requestID, status)
        {
            ConstructID = constructID;
        }

        public CreateConstructResponseMessage(JObject jobj) : base(jobj)
        {
            ConstructID = jobj.GetValue(MUSICJsonKeys.CONSTRUCT_ID).ToObject<EntityIDRecord>();
        }

        public CreateConstructResponseMessage(CreateConstructRequestMessage msg, EntityIDRecord constructID, 
            RequestStatus status)
            : base(msg, status)
        {
            ConstructID = constructID;
        }

        public override void AcceptVisitor(IMUSICMessageVisitor visitor)
        {
            visitor.VisitCreateConstructResponse(this);
        }

        public override object Clone()
        {
            return new CreateConstructResponseMessage
                (
                    MUSICHeader.ExerciseID,
                    (EntityIDRecord)OriginID.Clone(),
                    (EntityIDRecord)ReceiverID.Clone(),
                    RequestID,
                    (EntityIDRecord)ConstructID.Clone(),
                    RequestStatus
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
                { MUSICJsonKeys.STATUS, (int)RequestStatus },
                { MUSICJsonKeys.CONSTRUCT_ID, ConstructID.ToJsonObject() }
            };
        }

        public override bool Equals(object obj)
        {
            if (obj is CreateConstructResponseMessage other)
                if (base.Equals(other) && ConstructID == other.ConstructID)
                    return true;

            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = -99675531;

            hashCode = hashCode * -1521134295 + EqualityComparer<uint>.Default.GetHashCode(MUSICHeader.ExerciseID);
            hashCode = hashCode * -1521134295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(OriginID);
            hashCode = hashCode * -1521134295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(ReceiverID);
            hashCode = hashCode * -1521134295 + EqualityComparer<uint>.Default.GetHashCode(RequestID);
            hashCode = hashCode * -1521134295 + EqualityComparer<RequestStatus?>.Default.GetHashCode(RequestStatus);
            hashCode = hashCode * -1521134295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(ConstructID);

            return hashCode;
        }

        public static bool operator ==(CreateConstructResponseMessage message1, CreateConstructResponseMessage message2)
        {
            return EqualityComparer<CreateConstructResponseMessage>.Default.Equals(message1, message2);
        }

        public static bool operator !=(CreateConstructResponseMessage message1, CreateConstructResponseMessage message2)
        {
            return !(message1 == message2);
        }
    }
}
