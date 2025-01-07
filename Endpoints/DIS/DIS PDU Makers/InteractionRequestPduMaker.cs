/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using MUSICLibrary.MUSIC_Messages;
using MUSICLibrary.MUSIC_Messages.MUSIC_Request_Messages;
using OpenDis.Core;
using OpenDis.Dis1998;
using System;

namespace MUSICLibrary.Endpoints.DIS.DIS_PDU_Makers
{
    public class InteractionRequestPduMaker : PduMaker
    {
        public override byte[] MakeRaw(MUSICMessage message)
        {
            ActionRequestPdu pdu = new ActionRequestPdu();
            InteractionRequestMessage irMessage = (InteractionRequestMessage)message;

            pdu.ActionID = 77;
            pdu.ProtocolFamily = 42;

            pdu.ExerciseID = (byte)message.MUSICHeader.ExerciseID;
            pdu.RequestID = irMessage.RequestID;
            pdu.Timestamp = (uint)DateTimeOffset.Now.ToUnixTimeSeconds();

            var origin = message.OriginID;
            var receiver = irMessage.ReceiverID;

            pdu.OriginatingEntityID = new EntityID();
            pdu.OriginatingEntityID.Site = (ushort)origin.SiteID;
            pdu.OriginatingEntityID.Application = (ushort)origin.AppID;
            pdu.OriginatingEntityID.Entity = (ushort)origin.EntityID;

            pdu.ReceivingEntityID = new EntityID();
            pdu.ReceivingEntityID.Site = (ushort)receiver.SiteID;
            pdu.ReceivingEntityID.Application = (ushort)receiver.AppID;
            pdu.ReceivingEntityID.Entity = (ushort)receiver.EntityID;

            AddInteractionVariableDatumToActionRequest(pdu, irMessage.InteractionName);
            AddInteractionVariableDatumToActionRequest(pdu, irMessage.InteractionData.ToString());

            DataOutputStream dos = new DataOutputStream(Endian.Big);
            pdu.Marshal(dos);
            
            return dos.ConvertToBytes();
        }

        private void AddInteractionVariableDatumToActionRequest(ActionRequestPdu pdu, string payload)
        {
            var datum = new VariableDatum();
            datum.VariableDatumID = 454110001;
            AddStringToDatum(datum, payload);
            pdu.VariableDatums.Add(datum);
            pdu.NumberOfVariableDatumRecords += 1;
        }
    }
}
