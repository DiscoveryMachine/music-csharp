/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using MUSICLibrary.MUSIC_Messages;
using MUSICLibrary.MUSIC_Messages.MUSIC_Response_Messages;
using OpenDis.Core;
using OpenDis.Dis1998;

namespace MUSICLibrary.Endpoints.DIS.DIS_PDU_Makers
{
    public class InteractionResponsePduMaker : PduMaker
    {
        public override byte[] MakeRaw(MUSICMessage message)
        {
            InteractionResponseMessage response = (InteractionResponseMessage)message;

            ActionResponsePdu disPdu = new ActionResponsePdu
            {
                ExerciseID = (byte)response.MUSICHeader.ExerciseID,
                RequestStatus = (uint)response.RequestStatus,
                RequestID = response.RequestID
            };

            var originID = response.OriginID;
            var receiverID = response.ReceiverID;

            disPdu.OriginatingEntityID = new EntityID
            {
                Site = (ushort)originID.SiteID,
                Application = (ushort)originID.AppID,
                Entity = (ushort)originID.EntityID,
            };

            disPdu.ReceivingEntityID = new EntityID
            {
                Site = (ushort)receiverID.SiteID,
                Application = (ushort)receiverID.AppID,
                Entity = (ushort)receiverID.EntityID,
            };

            if (response.OptionalData != null)
            {
                var optionalData = new VariableDatum();
                optionalData.VariableDatumID = 454119000;
                AddStringToDatum(optionalData, response.OptionalData.ToString());
                disPdu.VariableDatums.Add(optionalData);
            }

            DataOutputStream dos = new DataOutputStream(Endian.Big);
            disPdu.Marshal(dos);
            return dos.ConvertToBytes();
        }
    }
}
