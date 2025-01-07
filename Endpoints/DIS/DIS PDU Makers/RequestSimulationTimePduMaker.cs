/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using MUSICLibrary.MUSIC_Messages;
using MUSICLibrary.MUSIC_Messages.Targeted_MUSIC_Messages;
using OpenDis.Core;
using OpenDis.Dis1998;

namespace MUSICLibrary.Endpoints.DIS.DIS_PDU_Makers
{
    public class RequestSimulationTimePduMaker : PduMaker
    {
        public override byte[] MakeRaw(MUSICMessage message)
        {
            SetDataPdu disPdu = new SetDataPdu();
            disPdu.ExerciseID = (byte)message.MUSICHeader.ExerciseID;
            var request = (RequestSimulationTimeMessage)message;

            disPdu.OriginatingEntityID = new EntityID
            {
                Site = (ushort)request.OriginID.SiteID,
                Application = (ushort)request.OriginID.AppID,
                Entity = (ushort)request.OriginID.EntityID
            };

            disPdu.ReceivingEntityID = new EntityID
            {
                Site = (ushort)request.ReceiverID.SiteID,
                Application = (ushort)request.ReceiverID.AppID,
                Entity = (ushort)request.ReceiverID.EntityID
            };

            disPdu.FixedDatums.Add(new FixedDatum
            {
                FixedDatumID = request.CommandIdentifier,
                FixedDatumValue = 0
            });

            DataOutputStream dos = new DataOutputStream(Endian.Big);
            disPdu.Marshal(dos);
            return dos.ConvertToBytes();
        }
    }
}
