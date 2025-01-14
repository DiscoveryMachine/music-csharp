﻿/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using MUSICLibrary.MUSIC_Messages.Targeted_MUSIC_Messages;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MUSICLibrary.MUSIC_Messages.MUSIC_Request_Messages
{
    public abstract class MUSICRequestMessage : TargetedMUSICMessage
    {
        private const string REQUEST_ID_KEY = "requestID";

        public uint RequestID { get; set; }

        public MUSICRequestMessage() { }

        public MUSICRequestMessage(uint exerciseID, EntityIDRecord originID, EntityIDRecord receiverID, uint requestID)
            : base(exerciseID, originID, receiverID)
        {
            RequestID = requestID;
        }

        public MUSICRequestMessage(JObject jobj)
            : base(jobj)
        {
            RequestID = jobj[REQUEST_ID_KEY].ToObject<uint>();
        }

        public override bool Equals(object obj)
        {
            if (obj is MUSICRequestMessage)
                return base.Equals(obj) && RequestID.Equals(((MUSICRequestMessage)obj).RequestID);
            else return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 1500581749;
            hashCode = hashCode * -1331134295 + EqualityComparer<uint>.Default.GetHashCode(MUSICHeader.ExerciseID);
            hashCode = hashCode * -1331134295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(OriginID);
            hashCode = hashCode * -1331134295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(ReceiverID);
            hashCode = hashCode * -1331134295 + EqualityComparer<uint>.Default.GetHashCode(RequestID);
            return hashCode;
        }

        public static bool operator ==(MUSICRequestMessage message1, MUSICRequestMessage message2)
        {
            return EqualityComparer<MUSICRequestMessage>.Default.Equals(message1, message2);
        }

        public static bool operator !=(MUSICRequestMessage message1, MUSICRequestMessage message2)
        {
            return !(message1 == message2);
        }
    }
}
