/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using MUSICLibrary.Interfaces;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace MUSICLibrary.MUSIC_Messages.Targeted_MUSIC_Messages
{
    public class DisplayMessagesMessage : TargetedMUSICMessage, IMUSICCommandMessage
    {
        public List<string> Messages { get; set; }
        public uint Timeout { get; set; }
        public uint CommandIdentifier => 454004005;

        public DisplayMessagesMessage(uint exerciseID, EntityIDRecord originID, EntityIDRecord recieverID,
            List<string> messages, uint timeout) : base(exerciseID, originID, recieverID)
        {
            Messages = messages;
            Timeout = timeout;
        }

        public DisplayMessagesMessage(JObject obj) : base(obj)
        {
            Messages = obj[MUSICJsonKeys.MESSAGES].ToObject<List<string>>();
            Timeout = obj[MUSICJsonKeys.TIMEOUT].ToObject<uint>();
        }

        public override void AcceptVisitor(IMUSICMessageVisitor visitor)
        {
            visitor.VisitDisplayMessages(this);
        }

        public override object Clone()
        {
            return new DisplayMessagesMessage
                (
                    MUSICHeader.ExerciseID,
                    (EntityIDRecord)OriginID.Clone(),
                    (EntityIDRecord)ReceiverID.Clone(),
                    Messages.ConvertAll(i => string.Copy(i)),
                    Timeout
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
                { MUSICJsonKeys.MESSAGES, JArray.FromObject(Messages) },
                { MUSICJsonKeys.TIMEOUT, Timeout }
            };
        }

        public override bool Equals(object obj)
        {
            var message = obj as DisplayMessagesMessage;
            return message != null &&
                   base.Equals(obj) &&
                   Messages.SequenceEqual(message.Messages) &&
                   Timeout == message.Timeout;
        }

        public override int GetHashCode()
        {
            var hashCode = 2120669259;

            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<List<string>>.Default.GetHashCode(Messages);
            hashCode = hashCode * -1521134295 + Timeout.GetHashCode();

            return hashCode;
        }

        public static bool operator ==(DisplayMessagesMessage message1, DisplayMessagesMessage message2)
        {
            return EqualityComparer<DisplayMessagesMessage>.Default.Equals(message1, message2);
        }

        public static bool operator !=(DisplayMessagesMessage message1, DisplayMessagesMessage message2)
        {
            return !(message1 == message2);
        }
    }
}
