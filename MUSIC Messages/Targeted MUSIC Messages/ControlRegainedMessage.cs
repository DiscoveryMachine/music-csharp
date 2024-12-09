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
    public class ControlRegainedMessage : ControlledConstructMessage
    {
        public ControlRegainedMessage(uint exerciseID, EntityIDRecord originID, 
            EntityIDRecord receiverID, EntityIDRecord controlledConstruct) : base(exerciseID, originID, receiverID, controlledConstruct)
        {

        }

        public ControlRegainedMessage(JObject obj) : base(obj)
        {

        }

        public override void AcceptVisitor(IMUSICMessageVisitor visitor)
        {
            visitor.VisitControlRegained(this);
        }

        public override object Clone()
        {
            return new ControlRegainedMessage
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
            if (obj is ControlRegainedMessage other)
                if (base.Equals(other))
                    return true;

            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 142424450;

            hashCode = hashCode * -1127144299 + EqualityComparer<uint>.Default.GetHashCode(MUSICHeader.ExerciseID);
            hashCode = hashCode * -1127144299 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(OriginID);
            hashCode = hashCode * -1127144299 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(ReceiverID);
            hashCode = hashCode * -1127144299 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(ControlledConstruct);

            return hashCode;
        }

        public static bool operator ==(ControlRegainedMessage message1, ControlRegainedMessage message2)
        {
            return EqualityComparer<ControlRegainedMessage>.Default.Equals(message1, message2);
        }

        public static bool operator !=(ControlRegainedMessage message1, ControlRegainedMessage message2)
        {
            return !(message1 == message2);
        }
    }
}
