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
    public class SimulationTimeMessage : TargetedMUSICMessage, IMUSICCommandMessage
    {
        public uint SimTime { get; set; }
        public uint CommandIdentifier => 454009999;

        public SimulationTimeMessage()
        {

        }

        public SimulationTimeMessage(uint exerciseID, EntityIDRecord originID, 
            EntityIDRecord recieverID, uint simTime) : base(exerciseID, originID, recieverID)
        {
            SimTime = simTime;
        }

        public SimulationTimeMessage(JObject obj) : base(obj)
        {
            SimTime = obj.GetValue("simTime").ToObject<uint>();
        }

        public override void AcceptVisitor(IMUSICMessageVisitor visitor)
        {
            visitor.VisitSimulationTime(this);
        }

        public override object Clone()
        {
            return new SimulationTimeMessage
                (
                    MUSICHeader.ExerciseID,
                    (EntityIDRecord)OriginID.Clone(), 
                    (EntityIDRecord)ReceiverID.Clone(),
                    SimTime
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
                { MUSICJsonKeys.SIM_TIME, SimTime }
            };
        }

        public override bool Equals(object obj)
        {
            if (obj is SimulationTimeMessage other)
                if (base.Equals(other) && SimTime == other.SimTime)
                    return true;

            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 1106421067;

            hashCode = hashCode * -1333334277 + EqualityComparer<uint>.Default.GetHashCode(MUSICHeader.ExerciseID);
            hashCode = hashCode * -1333334277 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(OriginID);
            hashCode = hashCode * -1333334277 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(ReceiverID);
            hashCode = hashCode * -1333334277 + EqualityComparer<uint>.Default.GetHashCode(SimTime);

            return hashCode;
        }

        public static bool operator ==(SimulationTimeMessage message1, SimulationTimeMessage message2)
        {
            return EqualityComparer<SimulationTimeMessage>.Default.Equals(message1, message2);
        }

        public static bool operator !=(SimulationTimeMessage message1, SimulationTimeMessage message2)
        {
            return !(message1 == message2);
        }
    }
}
