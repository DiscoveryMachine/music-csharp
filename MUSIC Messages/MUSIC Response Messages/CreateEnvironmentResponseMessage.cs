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
    public class CreateEnvironmentResponseMessage : MUSICResponseMessage, IMUSICCommandMessage
    {
        public uint CommandIdentifier => 454013001;

        protected CreateEnvironmentResponseMessage(uint exerciseID, EntityIDRecord originID, 
            EntityIDRecord receiverID, uint requestID, RequestStatus status) 
            : base(exerciseID, originID, receiverID, requestID, status)
        {

        }

        public CreateEnvironmentResponseMessage(JObject jobj) : base(jobj)
        {

        }

        public CreateEnvironmentResponseMessage(CreateEnvironmentRequestMessage msg, RequestStatus status) : base(msg, status)
        {
        }

        public override void AcceptVisitor(IMUSICMessageVisitor visitor)
        {
            visitor.VisitCreateEnvironmentResponse(this);
        }

        public override object Clone()
        {
            return new CreateEnvironmentResponseMessage
                (
                    MUSICHeader.ExerciseID,
                    (EntityIDRecord)OriginID.Clone(),
                    (EntityIDRecord)ReceiverID.Clone(),
                    RequestID,
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
                { MUSICJsonKeys.STATUS, (int)RequestStatus }
            };
        }

        public override bool Equals(object obj)
        {
            if (obj is CreateEnvironmentResponseMessage other)
                if (base.Equals(other))
                    return true;

            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 210212260;

            hashCode = hashCode * -1221134295 + EqualityComparer<uint>.Default.GetHashCode(MUSICHeader.ExerciseID);
            hashCode = hashCode * -1221134295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(OriginID);
            hashCode = hashCode * -1221134295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(ReceiverID);
            hashCode = hashCode * -1221134295 + EqualityComparer<uint>.Default.GetHashCode(RequestID);
            hashCode = hashCode * -1221134295 + EqualityComparer<RequestStatus?>.Default.GetHashCode(RequestStatus);

            return hashCode;
        }

        public static bool operator ==(CreateEnvironmentResponseMessage message1, CreateEnvironmentResponseMessage message2)
        {
            return EqualityComparer<CreateEnvironmentResponseMessage>.Default.Equals(message1, message2);
        }

        public static bool operator !=(CreateEnvironmentResponseMessage message1, CreateEnvironmentResponseMessage message2)
        {
            return !(message1 == message2);
        }
    }
}
