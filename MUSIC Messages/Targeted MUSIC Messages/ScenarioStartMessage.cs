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
    public class ScenarioStartMessage : TargetedMUSICMessage, IMUSICCommandMessage
    {
        public uint CommandIdentifier => 454013008;

        public ScenarioStartMessage(uint exerciseID, EntityIDRecord originID, 
            EntityIDRecord recieverID) : base(exerciseID, originID, recieverID)
        {

        }

        public ScenarioStartMessage(JObject obj) : base(obj)
        {

        }

        public override void AcceptVisitor(IMUSICMessageVisitor visitor)
        {
            visitor.VisitScenarioStart(this);
        }

        public override object Clone()
        {
            return new ScenarioStartMessage
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
                { MUSICJsonKeys.COMMAND_IDENTIFIER, CommandIdentifier },
                { MUSICJsonKeys.ORIGIN_ID, OriginID.ToJsonObject() },
                { MUSICJsonKeys.RECEIVER_ID, ReceiverID.ToJsonObject() }
            };
        }

        public override bool Equals(object obj)
        {
            if (obj is ScenarioStartMessage other)
                if (base.Equals(other))
                    return true;

            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = -122224544;

            hashCode = hashCode * -1127444295 + EqualityComparer<uint>.Default.GetHashCode(MUSICHeader.ExerciseID);
            hashCode = hashCode * -1127444295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(OriginID);
            hashCode = hashCode * -1127444295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(ReceiverID);

            return hashCode;
        }

        public static bool operator ==(ScenarioStartMessage message1, ScenarioStartMessage message2)
        {
            return EqualityComparer<ScenarioStartMessage>.Default.Equals(message1, message2);
        }

        public static bool operator !=(ScenarioStartMessage message1, ScenarioStartMessage message2)
        {
            return !(message1 == message2);
        }
    }
}
