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
    public abstract class TargetedMUSICMessage : MUSICMessage
    {
        public EntityIDRecord ReceiverID { get; set; }

        public TargetedMUSICMessage() { }

        public TargetedMUSICMessage(uint exerciseID, EntityIDRecord originID, EntityIDRecord recieverID)
            : base(exerciseID, originID)
        {
            this.ReceiverID = recieverID;
        }

        public TargetedMUSICMessage(MUSICHeader header, EntityIDRecord originID, EntityIDRecord receiverID)
            : base(header, originID)
        {
            this.ReceiverID = receiverID;
        }

        public TargetedMUSICMessage(JObject obj)
            : base(obj.GetValue("header").ToObject<MUSICHeader>(), obj.GetValue("originID").ToObject<EntityIDRecord>())
        {
            ReceiverID = obj.GetValue("receiverID").ToObject<EntityIDRecord>();
        }

        public override bool Equals(object obj)
        {
            if (obj is TargetedMUSICMessage)
                return base.Equals(obj) && ReceiverID.Equals(((TargetedMUSICMessage)obj).ReceiverID);
            else return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 1839857571;
            hashCode = hashCode * -1521134000 + EqualityComparer<uint>.Default.GetHashCode(MUSICHeader.ExerciseID);
            hashCode = hashCode * -1521134000 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(OriginID);
            hashCode = hashCode * -1521134000 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(ReceiverID);
            return hashCode;
        }

        public static bool operator ==(TargetedMUSICMessage message1, TargetedMUSICMessage message2)
        {
            return EqualityComparer<TargetedMUSICMessage>.Default.Equals(message1, message2);
        }

        public static bool operator !=(TargetedMUSICMessage message1, TargetedMUSICMessage message2)
        {
            return !(message1 == message2);
        }
    }
}
