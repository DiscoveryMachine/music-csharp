/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System.Collections.Generic;
using MUSICLibrary.Interfaces;
using Newtonsoft.Json.Linq;

namespace MUSICLibrary.MUSIC_Messages.MUSIC_Request_Messages
{
    public class FinalizeScenarioRequestMessage : MUSICRequestMessage, IMUSICCommandMessage
    {
        public uint CommandIdentifier => 454013006;

        public FinalizeScenarioRequestMessage(uint exerciseID, EntityIDRecord originID,
            EntityIDRecord receiverID, uint requestID) 
            : base(exerciseID, originID, receiverID, requestID)
        {

        }

        public FinalizeScenarioRequestMessage(JObject jobj) : base(jobj)
        {

        }

        public override void AcceptVisitor(IMUSICMessageVisitor visitor)
        {
            visitor.VisitFinalizeScenarioRequest(this);
        }

        public override object Clone()
        {
            return new FinalizeScenarioRequestMessage
                (
                    MUSICHeader.ExerciseID,
                    (EntityIDRecord)OriginID.Clone(),
                    (EntityIDRecord)ReceiverID.Clone(),
                    RequestID
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
                { MUSICJsonKeys.REQUEST_ID, RequestID }
            };
        }

        public override bool Equals(object obj)
        {
            if (obj is FinalizeScenarioRequestMessage)
            {
                FinalizeScenarioRequestMessage other = (FinalizeScenarioRequestMessage)obj;

                if (base.Equals(other))
                    return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 440219460;

            hashCode = hashCode * -1001134695 + EqualityComparer<uint>.Default.GetHashCode(MUSICHeader.ExerciseID);
            hashCode = hashCode * -1001134695 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(OriginID);
            hashCode = hashCode * -1001134695 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(ReceiverID);
            hashCode = hashCode * -1001134695 + EqualityComparer<uint>.Default.GetHashCode(RequestID);

            return hashCode;
        }

        public static bool operator ==(FinalizeScenarioRequestMessage message1, FinalizeScenarioRequestMessage message2)
        {
            return EqualityComparer<FinalizeScenarioRequestMessage>.Default.Equals(message1, message2);
        }

        public static bool operator !=(FinalizeScenarioRequestMessage message1, FinalizeScenarioRequestMessage message2)
        {
            return !(message1 == message2);
        }
    }
}
