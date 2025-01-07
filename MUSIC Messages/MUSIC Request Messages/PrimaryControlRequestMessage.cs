/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System.Collections.Generic;
using MUSICLibrary.Interfaces;
using Newtonsoft.Json.Linq;

namespace MUSICLibrary.MUSIC_Messages.MUSIC_Request_Messages
{
    public class PrimaryControlRequestMessage : MUSICRequestMessage
    {
        public PrimaryControlRequestMessage(uint exerciseID, EntityIDRecord originID,
            EntityIDRecord receiverID, uint requestID)
            : base(exerciseID, originID, receiverID, requestID)
        {

        }

        public PrimaryControlRequestMessage(JObject jobj)
            : base(jobj)
        {

        }

        public override void AcceptVisitor(IMUSICMessageVisitor visitor)
        {
            visitor.VisitPrimaryControlRequest(this);
        }

        public override object Clone()
        {
            return new PrimaryControlRequestMessage
                (
                    MUSICHeader.ExerciseID,
                    (EntityIDRecord)OriginID.Clone(),
                    (EntityIDRecord)ReceiverID.Clone(), 
                    RequestID
                );
        }

        public override JObject ToJsonObject()
        {
            return new JObject
            {
                { MUSICJsonKeys.HEADER, MUSICHeader.ToJsonObject() },
                { MUSICJsonKeys.ORIGIN_ID, OriginID.ToJsonObject() },
                { MUSICJsonKeys.RECEIVER_ID, ReceiverID.ToJsonObject() },
                { MUSICJsonKeys.REQUEST_ID, RequestID }
            };
        }

        public override bool Equals(object obj)
        {
            if (obj is PrimaryControlRequestMessage other)
                if (base.Equals(other))
                    return true;

            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 122224500;

            hashCode = hashCode * -1020134295 + EqualityComparer<uint>.Default.GetHashCode(MUSICHeader.ExerciseID);
            hashCode = hashCode * -1020134295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(OriginID);
            hashCode = hashCode * -1020134295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(ReceiverID);
            hashCode = hashCode * -1020134295 + EqualityComparer<uint>.Default.GetHashCode(RequestID);

            return hashCode;
        }

        public static bool operator ==(PrimaryControlRequestMessage message1, PrimaryControlRequestMessage message2)
        {
            return EqualityComparer<PrimaryControlRequestMessage>.Default.Equals(message1, message2);
        }

        public static bool operator !=(PrimaryControlRequestMessage message1, PrimaryControlRequestMessage message2)
        {
            return !(message1 == message2);
        }
    }
}
