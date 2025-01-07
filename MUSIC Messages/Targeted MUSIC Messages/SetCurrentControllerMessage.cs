/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using MUSICLibrary.Interfaces;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MUSICLibrary.MUSIC_Messages.Targeted_MUSIC_Messages
{
    public class SetCurrentControllerMessage : TargetedMUSICMessage
    {
        public EntityIDRecord CurrentController { get; set; }

        public SetCurrentControllerMessage(uint exerciseID, EntityIDRecord originID, EntityIDRecord recieverID,
            EntityIDRecord currentController) : base(exerciseID, originID, recieverID)
        {
            CurrentController = currentController;
        }

        public SetCurrentControllerMessage(JObject obj) : base(obj)
        {
            CurrentController = obj.GetValue(MUSICJsonKeys.CURRENT_CONTROLLER_ID).ToObject<EntityIDRecord>();
        }

        public override void AcceptVisitor(IMUSICMessageVisitor visitor)
        {
            visitor.VisitSetCurrentController(this);
        }

        public override object Clone()
        {
            return new SetCurrentControllerMessage
                (
                    MUSICHeader.ExerciseID,
                    (EntityIDRecord)OriginID.Clone(),
                    (EntityIDRecord)ReceiverID.Clone(),
                    (EntityIDRecord)CurrentController.Clone()
                );
        }

        public override JObject ToJsonObject()
        {
            return new JObject
            {
                { MUSICJsonKeys.HEADER, MUSICHeader.ToJsonObject() },
                { MUSICJsonKeys.ORIGIN_ID, OriginID.ToJsonObject() },
                { MUSICJsonKeys.RECEIVER_ID, ReceiverID.ToJsonObject() },
                { MUSICJsonKeys.CURRENT_CONTROLLER_ID, CurrentController.ToJsonObject() }
            };
        }

        public override bool Equals(object obj)
        {
            var message = obj as SetCurrentControllerMessage;
            return message != null &&
                   base.Equals(obj) &&
                   EqualityComparer<EntityIDRecord>.Default.Equals(CurrentController, message.CurrentController);
        }

        public override int GetHashCode()
        {
            var hashCode = -1526957576;

            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(CurrentController);

            return hashCode;
        }

        public static bool operator ==(SetCurrentControllerMessage message1, SetCurrentControllerMessage message2)
        {
            return EqualityComparer<SetCurrentControllerMessage>.Default.Equals(message1, message2);
        }

        public static bool operator !=(SetCurrentControllerMessage message1, SetCurrentControllerMessage message2)
        {
            return !(message1 == message2);
        }
    }
}
