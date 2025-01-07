/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using MUSICLibrary.Interfaces;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MUSICLibrary.MUSIC_Messages.Targeted_MUSIC_Messages
{
    public class ControlLostMessage : ControlledConstructMessage
    {
        public ControlLostMessage(uint exerciseID, EntityIDRecord originID, 
            EntityIDRecord receiverID, EntityIDRecord controlledConstruct) : base(exerciseID, originID, receiverID, controlledConstruct)
        {
        }

        public ControlLostMessage(JObject obj) : base(obj)
        {
        }

        public override void AcceptVisitor(IMUSICMessageVisitor visitor)
        {
            visitor.VisitControlLost(this);
        }

        public override object Clone()
        {
            return new ControlLostMessage
                (
                    MUSICHeader.ExerciseID,
                    (EntityIDRecord)OriginID.Clone(),
                    (EntityIDRecord)ReceiverID.Clone(),
                    (EntityIDRecord)ControlledConstruct.Clone()
                );
        }

        public override JObject ToJsonObject()
        {
            return new JObject
            {
                { MUSICJsonKeys.HEADER, MUSICHeader.ToJsonObject() },
                { MUSICJsonKeys.ORIGIN_ID, OriginID.ToJsonObject() },
                { MUSICJsonKeys.RECEIVER_ID, ReceiverID.ToJsonObject() },
                { MUSICJsonKeys.CONTROLLED_CONSTRUCT, ControlledConstruct.ToJsonObject() }
            };
        }

        public override bool Equals(object obj)
        {
            if (obj is ControlLostMessage other)
                if (base.Equals(other))
                    return true;

            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 129994559;

            hashCode = hashCode * -1417194105 + EqualityComparer<uint>.Default.GetHashCode(MUSICHeader.ExerciseID);
            hashCode = hashCode * -1417194105 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(OriginID);
            hashCode = hashCode * -1417194105 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(ReceiverID);
            hashCode = hashCode * -1417194105 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(ControlledConstruct);

            return hashCode;
        }

        public static bool operator ==(ControlLostMessage message1, ControlLostMessage message2)
        {
            return EqualityComparer<ControlLostMessage>.Default.Equals(message1, message2);
        }

        public static bool operator !=(ControlLostMessage message1, ControlLostMessage message2)
        {
            return !(message1 == message2);
        }
    }
}
