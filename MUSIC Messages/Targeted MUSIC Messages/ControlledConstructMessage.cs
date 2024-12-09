//=====================================================================
//DISCOVERY MACHINE, INC. PROPRIETARY INFORMATION
//
//This software is supplied under the terms of a license agreement
//or nondisclosure agreement with Discovery Machine, Inc. and may
//not be copied or disclosed except in accordance with the terms  of
//that agreement.
//
//Copyright 2022 Discovery Machine, Inc. All Rights Reserved.
//=====================================================================

using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MUSICLibrary.MUSIC_Messages.Targeted_MUSIC_Messages
{
    public abstract class ControlledConstructMessage : TargetedMUSICMessage
    {
        public EntityIDRecord ControlledConstruct { get; set; }

        public ControlledConstructMessage(uint exerciseID, 
            EntityIDRecord originID, EntityIDRecord receiverID, 
            EntityIDRecord controlledConstruct) 
            : base(exerciseID, originID, receiverID)
        {
            ControlledConstruct = controlledConstruct;
        }

        public ControlledConstructMessage(MUSICHeader header, EntityIDRecord originID, EntityIDRecord receiverID, EntityIDRecord controlledConstruct)
            : base(header, originID, receiverID)
        {
            ControlledConstruct = controlledConstruct;
        }

        public ControlledConstructMessage(JObject obj) : base(obj)
        {
            ControlledConstruct = obj.GetValue("controlledConstruct").ToObject<EntityIDRecord>();
        }

        public override bool Equals(object obj)
        {
            if (obj is ControlledConstructMessage)
                return base.Equals(obj) && ControlledConstruct.Equals(((ControlledConstructMessage)obj).ControlledConstruct);
            else return false;
        }

        public override int GetHashCode()
        {
            var hashCode = -886494282;
            hashCode = hashCode * -1020133995 + EqualityComparer<uint>.Default.GetHashCode(MUSICHeader.ExerciseID);
            hashCode = hashCode * -1020133995 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(OriginID);
            hashCode = hashCode * -1020133995 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(ReceiverID);
            hashCode = hashCode * -1020133995 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(ControlledConstruct);
            return hashCode;
        }

        public static bool operator ==(ControlledConstructMessage message1, ControlledConstructMessage message2)
        {
            return EqualityComparer<ControlledConstructMessage>.Default.Equals(message1, message2);
        }

        public static bool operator !=(ControlledConstructMessage message1, ControlledConstructMessage message2)
        {
            return !(message1 == message2);
        }
    }
}
