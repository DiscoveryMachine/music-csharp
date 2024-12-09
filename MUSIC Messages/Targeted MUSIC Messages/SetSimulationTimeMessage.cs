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

using MUSICLibrary.Interfaces;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MUSICLibrary.MUSIC_Messages.Targeted_MUSIC_Messages
{
    public class SetSimulationTimeMessage : TargetedMUSICMessage, IMUSICCommandMessage
    {
        public uint NewTime { get; set; }
        public uint CommandIdentifier => 454009001;

        public SetSimulationTimeMessage(uint exerciseID, EntityIDRecord originID, EntityIDRecord recieverID,
            uint newTime) : base(exerciseID, originID, recieverID)
        {
            NewTime = newTime;
        }

        public SetSimulationTimeMessage(JObject obj) : base(obj)
        {
            NewTime = obj.GetValue(MUSICJsonKeys.NEW_TIME).ToObject<uint>();
        }

        public override void AcceptVisitor(IMUSICMessageVisitor visitor)
        {
            visitor.VisitSetSimulationTime(this);
        }

        public override object Clone()
        {
            return new SetSimulationTimeMessage
                (
                    MUSICHeader.ExerciseID,
                    (EntityIDRecord)OriginID.Clone(),
                    (EntityIDRecord)ReceiverID.Clone(),
                    NewTime
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
                { MUSICJsonKeys.NEW_TIME, NewTime }
            };
        }

        public override bool Equals(object obj)
        {
            if (obj is SetSimulationTimeMessage other)
                if (base.Equals(other) && NewTime == other.NewTime)
                    return true;

            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 2047867454;

            hashCode = hashCode * -1521134295 + EqualityComparer<uint>.Default.GetHashCode(MUSICHeader.ExerciseID);
            hashCode = hashCode * -1521134295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(OriginID);
            hashCode = hashCode * -1521134295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(ReceiverID);
            hashCode = hashCode * -1521134295 + EqualityComparer<uint>.Default.GetHashCode(NewTime);

            return hashCode;
        }

        public static bool operator ==(SetSimulationTimeMessage message1, SetSimulationTimeMessage message2)
        {
            return EqualityComparer<SetSimulationTimeMessage>.Default.Equals(message1, message2);
        }

        public static bool operator !=(SetSimulationTimeMessage message1, SetSimulationTimeMessage message2)
        {
            return !(message1 == message2);
        }
    }
}
