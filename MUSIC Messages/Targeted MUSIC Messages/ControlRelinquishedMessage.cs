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
    public class ControlRelinquishedMessage : TargetedMUSICMessage
    {
        public ControlRelinquishedMessage(uint exerciseID, EntityIDRecord originID, EntityIDRecord recieverID) 
            : base(exerciseID, originID, recieverID)
        {

        }

        public ControlRelinquishedMessage(JObject obj) : base(obj)
        {

        }

        public override void AcceptVisitor(IMUSICMessageVisitor visitor)
        {
            visitor.VisitControlRelinquished(this);
        }

        public override object Clone()
        {
            return new ControlRelinquishedMessage
                (
                    MUSICHeader.ExerciseID,
                    (EntityIDRecord)OriginID.Clone(),
                    (EntityIDRecord)ReceiverID.Clone()
                );
        }

        public override JObject ToJsonObject()
        {
            return new JObject
            {
                { MUSICJsonKeys.HEADER, MUSICHeader.ToJsonObject() },
                { MUSICJsonKeys.ORIGIN_ID, OriginID.ToJsonObject() },
                { MUSICJsonKeys.RECEIVER_ID, ReceiverID.ToJsonObject() }
            };
        }

        public override bool Equals(object obj)
        {
            if (obj is ControlRelinquishedMessage other)
                if (base.Equals(other))
                    return true;

            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 335924550;

            hashCode = hashCode * -1521124292 + EqualityComparer<uint>.Default.GetHashCode(MUSICHeader.ExerciseID);
            hashCode = hashCode * -1521124292 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(OriginID);
            hashCode = hashCode * -1521124292 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(ReceiverID);

            return hashCode;
        }

        public static bool operator ==(ControlRelinquishedMessage message1, ControlRelinquishedMessage message2)
        {
            return EqualityComparer<ControlRelinquishedMessage>.Default.Equals(message1, message2);
        }

        public static bool operator !=(ControlRelinquishedMessage message1, ControlRelinquishedMessage message2)
        {
            return !(message1 == message2);
        }
    }
}
