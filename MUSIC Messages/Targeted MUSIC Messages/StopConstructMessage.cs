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
    public class StopConstructMessage : TargetedMUSICMessage, IMUSICCommandMessage
    {
        public uint CommandIdentifier => 454007001;

        public StopConstructMessage(uint exerciseID, EntityIDRecord originID, 
            EntityIDRecord recieverID) : base(exerciseID, originID, recieverID)
        {

        }

        public StopConstructMessage(JObject obj) : base(obj) { }

        public override void AcceptVisitor(IMUSICMessageVisitor visitor)
        {
            visitor.VisitStopConstruct(this);
        }

        public override object Clone()
        {
            return new StopConstructMessage
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
            if (obj is StopConstructMessage other)
                if (base.Equals(other))
                    return true;

            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 12222687;

            hashCode = hashCode * -1127555295 + EqualityComparer<uint>.Default.GetHashCode(MUSICHeader.ExerciseID);
            hashCode = hashCode * -1127555295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(OriginID);
            hashCode = hashCode * -1127555295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(ReceiverID);

            return hashCode;
        }

        public static bool operator ==(StopConstructMessage message1, StopConstructMessage message2)
        {
            return EqualityComparer<StopConstructMessage>.Default.Equals(message1, message2);
        }

        public static bool operator !=(StopConstructMessage message1, StopConstructMessage message2)
        {
            return !(message1 == message2);
        }
    }
}
