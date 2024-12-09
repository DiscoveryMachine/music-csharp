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

using MUSICLibrary.MUSIC_Messages.MUSIC_Request_Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MUSICLibrary.MUSIC_Messages.MUSIC_Response_Messages
{
    public abstract class MUSICResponseMessage : MUSICRequestMessage
    {
        private const string STATUS_KEY = "status";

        [JsonProperty(STATUS_KEY)]
        public RequestStatus RequestStatus { get; set; }

        protected MUSICResponseMessage(uint exerciseID, EntityIDRecord originID,
            EntityIDRecord receiverID, uint requestID, RequestStatus status)
            : base(exerciseID, originID, receiverID, requestID)
        {
            RequestStatus = status;
        }

        public MUSICResponseMessage() { }

        public MUSICResponseMessage(JObject jobj) : base(jobj)
        {
            RequestStatus = jobj[STATUS_KEY].ToObject<RequestStatus>();
        }

        //Only switches when constructing a response message from a request message
        private MUSICResponseMessage(MUSICRequestMessage msg)
            : base(msg.MUSICHeader.ExerciseID, msg.ReceiverID, msg.OriginID, msg.RequestID) // This base constructor swaps origin and receiver IDs. It is not a mistake.
        {
        }

        public MUSICResponseMessage(MUSICRequestMessage msg, RequestStatus requestStatus) : this(msg)
        {
            RequestStatus = requestStatus;
        }

        public override bool Equals(object obj)
        {
            if (obj is MUSICResponseMessage)
                return base.Equals(obj) && RequestStatus.Equals(((MUSICResponseMessage)obj).RequestStatus);
            else return false;
        }

        public override int GetHashCode()
        {
            var hashCode = -1250670558;
            hashCode = hashCode * -1121134295 + EqualityComparer<uint>.Default.GetHashCode(MUSICHeader.ExerciseID);
            hashCode = hashCode * -1121134295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(OriginID);
            hashCode = hashCode * -1121134295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(ReceiverID);
            hashCode = hashCode * -1121134295 + EqualityComparer<uint>.Default.GetHashCode(RequestID);
            hashCode = hashCode * -1121134295 + EqualityComparer<RequestStatus?>.Default.GetHashCode(RequestStatus);
            return hashCode;
        }

        public static bool operator ==(MUSICResponseMessage message1, MUSICResponseMessage message2)
        {
            return EqualityComparer<MUSICResponseMessage>.Default.Equals(message1, message2);
        }

        public static bool operator !=(MUSICResponseMessage message1, MUSICResponseMessage message2)
        {
            return !(message1 == message2);
        }
    }
}
