//=====================================================================
// DISCOVERY MACHINE, INC. PROPRIETARY INFORMATION
//
// This software is supplied under the terms of a license agreement
// or nondisclosure agreement with Discovery Machine, Inc. and may
// not be copied or disclosed except in accordance with the terms of
// that agreement.
//
// Copyright 2023 Discovery Machine, Inc. All Rights Reserved.
//=====================================================================

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
