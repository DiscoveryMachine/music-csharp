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
using OpenDis.Core;
using OpenDis.Dis1998;
using System;

namespace MUSICLibrary.Endpoints.DIS.DIS_PDU_Makers
{
    public class StateFieldPduMaker : PduMaker
    {
        public override byte[] MakeRaw(MUSICMessage message)
        {
            StateFieldMessage stateField = (StateFieldMessage)message;
            StateFieldPdu pdu = new StateFieldPdu
            {
                ProtocolVersion = 6,
                ExerciseID = (byte)stateField.MUSICHeader.ExerciseID,
                PduType = 233,
                ProtocolFamily = 42,
                Timestamp = (uint)DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Padding = 0,
                ConstructID = new EntityID
                {
                    Site = (ushort)message.OriginID.SiteID,
                    Application = (ushort)message.OriginID.AppID,
                    Entity = (ushort)message.OriginID.EntityID,
                },
                Payload = stateField.StateDataObject,
            };

            pdu.Length = (ushort)pdu.GetMarshalledSize();

            DataOutputStream dos = new DataOutputStream(Endian.Big);
            pdu.Marshal(dos);
            return dos.ConvertToBytes();
        }
    }
}
