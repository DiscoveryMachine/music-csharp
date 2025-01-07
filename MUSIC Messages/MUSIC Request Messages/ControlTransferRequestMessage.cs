/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using MUSICLibrary.Interfaces;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MUSICLibrary.MUSIC_Messages.MUSIC_Request_Messages
{
    public class ControlTransferRequestMessage : ControlRequestMessage
    {
        public EntityIDRecord ProposedController { get; private set; }

        public ControlTransferRequestMessage(uint exerciseID, EntityIDRecord originID,
            EntityIDRecord receiverID, uint requestID, EntityIDRecord targetConstruct,
            EntityIDRecord proposedController, JObject context = null)
            : base(exerciseID, originID, receiverID, requestID, targetConstruct, context)
        {
            ProposedController = proposedController;
        }

        public ControlTransferRequestMessage(JObject jobj) : base(jobj)
        {
            ProposedController = jobj[MUSICJsonKeys.PROPOSED_CONTROLLER].ToObject<EntityIDRecord>();
        }

        public override void AcceptVisitor(IMUSICMessageVisitor visitor)
        {
            visitor.VisitControlTransferRequest(this);
        }

        public override object Clone()
        {
            return new ControlTransferRequestMessage
                (
                    MUSICHeader.ExerciseID,
                    (EntityIDRecord)OriginID.Clone(),
                    (EntityIDRecord)ReceiverID.Clone(), 
                    RequestID,
                    (EntityIDRecord)TargetConstruct.Clone(),
                    (EntityIDRecord)ProposedController.Clone(),
                    (JObject)Context.DeepClone()
                );
        }

        public override JObject ToJsonObject()
        {
            return new JObject
            {
                { MUSICJsonKeys.HEADER, MUSICHeader.ToJsonObject() },
                { MUSICJsonKeys.ORIGIN_ID, OriginID.ToJsonObject() },
                { MUSICJsonKeys.RECEIVER_ID, ReceiverID.ToJsonObject() },
                { MUSICJsonKeys.REQUEST_ID, RequestID },
                { MUSICJsonKeys.TARGET_CONSTRUCT, TargetConstruct.ToJsonObject() },
                { MUSICJsonKeys.PROPOSED_CONTROLLER, ProposedController.ToJsonObject() },
                { MUSICJsonKeys.CONTEXT, Context }
            };
        }

        public override bool Equals(object obj)
        {
            if (obj is ControlTransferRequestMessage other)
                if (base.Equals(other) && ProposedController == other.ProposedController)
                    return true;

            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = -500387931;

            hashCode = hashCode * -1521134211 + EqualityComparer<uint>.Default.GetHashCode(MUSICHeader.ExerciseID);
            hashCode = hashCode * -1521134211 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(OriginID);
            hashCode = hashCode * -1521134211 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(ReceiverID);
            hashCode = hashCode * -1521134211 + EqualityComparer<uint>.Default.GetHashCode(RequestID);
            hashCode = hashCode * -1521134211 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(TargetConstruct);
            hashCode = hashCode * -1521134211 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(ProposedController);
            hashCode = hashCode * -1521134211 + EqualityComparer<string>.Default.GetHashCode(Context.ToString());

            return hashCode;
        }

        public static bool operator ==(ControlTransferRequestMessage message1, ControlTransferRequestMessage message2)
        {
            return EqualityComparer<ControlTransferRequestMessage>.Default.Equals(message1, message2);
        }

        public static bool operator !=(ControlTransferRequestMessage message1, ControlTransferRequestMessage message2)
        {
            return !(message1 == message2);
        }
    }
}
