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
    public class CreateEnvironmentRequestMessage : MUSICRequestMessage, IMUSICCommandMessage
    {
        public string EnvironmentName { get; set; }
        public string EnvironmentMetaData { get; set; }
        public uint CommandIdentifier => 454013000;

        public CreateEnvironmentRequestMessage(JObject jobj) : base(jobj)
        {
            EnvironmentName = jobj.GetValue(MUSICJsonKeys.ENVIRONMENT_NAME).ToObject<string>();
            EnvironmentMetaData = jobj.GetValue(MUSICJsonKeys.ENVIRONMENT_META_DATA).ToObject<string>();
        }

        public CreateEnvironmentRequestMessage(uint exerciseID, 
            EntityIDRecord originID, EntityIDRecord receiverID, uint requestID,
            string environmentName, string environmentMetaData) 
            : base(exerciseID, originID, receiverID, requestID)
        {
            EnvironmentName = environmentName;
            EnvironmentMetaData = environmentMetaData;
        }

        public override void AcceptVisitor(IMUSICMessageVisitor visitor)
        {
            visitor.VisitCreateEnvironmentRequest(this);
        }

        public override object Clone()
        {
            return new CreateEnvironmentRequestMessage
                (
                    MUSICHeader.ExerciseID,
                    (EntityIDRecord)OriginID.Clone(),
                    (EntityIDRecord)ReceiverID.Clone(),
                    RequestID,
                    string.Copy(EnvironmentName),
                    EnvironmentMetaData
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
                { MUSICJsonKeys.REQUEST_ID, RequestID },
                { MUSICJsonKeys.ENVIRONMENT_NAME, EnvironmentName },
                { MUSICJsonKeys.ENVIRONMENT_META_DATA, EnvironmentMetaData }
            };
        }

        public override bool Equals(object obj)
        {
            if (obj is CreateEnvironmentRequestMessage)
            {
                CreateEnvironmentRequestMessage other = (CreateEnvironmentRequestMessage)obj;

                if (
                    base.Equals(other) &&
                    EnvironmentName.Equals(other.EnvironmentName) &&
                    JObject.DeepEquals(EnvironmentMetaData, other.EnvironmentMetaData)
                    )
                    return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = -968179538;

            hashCode = hashCode * -1001134095 + EqualityComparer<uint>.Default.GetHashCode(MUSICHeader.ExerciseID);
            hashCode = hashCode * -1001134095 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(OriginID);
            hashCode = hashCode * -1001134095 + EqualityComparer<EntityIDRecord>.Default.GetHashCode(ReceiverID);
            hashCode = hashCode * -1001134095 + EqualityComparer<uint>.Default.GetHashCode(RequestID);
            hashCode = hashCode * -1001134095 + EqualityComparer<string>.Default.GetHashCode(EnvironmentName);
            hashCode = hashCode * -1001134095 + EqualityComparer<string>.Default.GetHashCode(EnvironmentMetaData.ToString());

            return hashCode;
        }

        public static bool operator ==(CreateEnvironmentRequestMessage message1, CreateEnvironmentRequestMessage message2)
        {
            return EqualityComparer<CreateEnvironmentRequestMessage>.Default.Equals(message1, message2);
        }

        public static bool operator !=(CreateEnvironmentRequestMessage message1, CreateEnvironmentRequestMessage message2)
        {
            return !(message1 == message2);
        }
    }
}
