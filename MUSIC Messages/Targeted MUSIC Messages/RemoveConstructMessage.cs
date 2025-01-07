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
    public class RemoveConstructMessage : TargetedMUSICMessage, IMUSICCommandMessage
    {
        public EntityIDRecord RemovedConstruct { get; set; }
        public uint CommandIdentifier => 454007002;

        public RemoveConstructMessage(uint exerciseID, EntityIDRecord originID, 
            EntityIDRecord recieverID, EntityIDRecord removeConstruct) : base(exerciseID, originID, recieverID)
        {
            RemovedConstruct = removeConstruct;
        }

        public RemoveConstructMessage(JObject obj) : base(obj)
        {
            RemovedConstruct = obj.GetValue(MUSICJsonKeys.REMOVED_CONSTRUCT).ToObject<EntityIDRecord>();
        }

        public override void AcceptVisitor(IMUSICMessageVisitor visitor)
        {
            visitor.VisitRemoveConstruct(this);
        }

        public override object Clone()
        {
            return new RemoveConstructMessage
                (
                    MUSICHeader.ExerciseID,
                    (EntityIDRecord)OriginID.Clone(),
                    (EntityIDRecord)ReceiverID.Clone(),
                    (EntityIDRecord)RemovedConstruct.Clone()
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
                { MUSICJsonKeys.REMOVED_CONSTRUCT, RemovedConstruct.ToJsonObject() }
            };
        }

        public override bool Equals(object obj)
        {
            if (obj is RemoveConstructMessage other)
                if (base.Equals(other) && RemovedConstruct == other.RemovedConstruct)
                    return true;

            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = -1819630550;

            hashCode = hashCode * -1521134295 + EqualityComparer<uint>.Default.GetHashCode(MUSICHeader.ExerciseID);
            hashCode = hashCode * -1521134295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(OriginID);
            hashCode = hashCode * -1521134295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(ReceiverID);
            hashCode = hashCode * -1521134295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(RemovedConstruct);

            return hashCode;
        }

        public static bool operator ==(RemoveConstructMessage message1, RemoveConstructMessage message2)
        {
            return EqualityComparer<RemoveConstructMessage>.Default.Equals(message1, message2);
        }

        public static bool operator !=(RemoveConstructMessage message1, RemoveConstructMessage message2)
        {
            return !(message1 == message2);
        }
    }
}
