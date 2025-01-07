/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

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
