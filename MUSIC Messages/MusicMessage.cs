//=====================================================================
//DISCOVERY MACHINE, INC. PROPRIETARY INFORMATION
//
//This software is supplied under the terms of a license agreement
//or nondisclosure agreement with Discovery Machine, Inc. and may
//not be copied or disclosed except in accordance with the terms of
//that agreement.
//
//Copyright 2022 Discovery Machine, Inc. All Rights Reserved.
//=====================================================================

using System;
using System.Collections.Generic;
using MUSICLibrary.Interfaces;
using Newtonsoft.Json.Linq;

namespace MUSICLibrary.MUSIC_Messages
{
    public abstract class MUSICMessage : IMessagePrototype
    {
        public MUSICHeader MUSICHeader { get; set; }
        public EntityIDRecord OriginID { get; set; }

        public MUSICMessage() { }

        public MUSICMessage(uint exerciseID, EntityIDRecord originID)
        {
            MUSICHeader = new MUSICHeader(exerciseID, (ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            OriginID = originID;
        }

        public MUSICMessage(MUSICHeader header, EntityIDRecord originID)
        {
            MUSICHeader = header;
            OriginID = originID;
        }

        //Should not reach this point. Function should be overridden
        public abstract void AcceptVisitor(IMUSICMessageVisitor visitor);

        //Should not reach this point. Function should be overridden
        public abstract object Clone();

        //Should not reach this point. Function should be overridden
        public abstract JObject ToJsonObject();

        public override bool Equals(object obj)
        {
            var message = obj as MUSICMessage;
            return message != null &&
                   EqualityComparer<MUSICHeader>.Default.Equals(MUSICHeader, message.MUSICHeader) &&
                   EqualityComparer<EntityIDRecord>.Default.Equals(OriginID, message.OriginID);
        }

        public override int GetHashCode()
        {
            var hashCode = 1477022203;
            hashCode = hashCode * -1521134295 + EqualityComparer<MUSICHeader>.Default.GetHashCode(MUSICHeader);
            hashCode = hashCode * -1521134295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(OriginID);
            return hashCode;
        }

        public static bool operator ==(MUSICMessage message1, MUSICMessage message2)
        {
            return EqualityComparer<MUSICMessage>.Default.Equals(message1, message2);
        }

        public static bool operator !=(MUSICMessage message1, MUSICMessage message2)
        {
            return !(message1 == message2);
        }
    }
}
