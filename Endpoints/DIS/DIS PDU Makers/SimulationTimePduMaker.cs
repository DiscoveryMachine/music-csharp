/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using MUSICLibrary.MUSIC_Messages;
using MUSICLibrary.MUSIC_Messages.Targeted_MUSIC_Messages;
using Newtonsoft.Json.Linq;
using OpenDis.Core;
using OpenDis.Dis1998;

namespace MUSICLibrary.Endpoints.DIS.DIS_PDU_Makers
{
    public class SimulationTimePduMaker : PduMaker
    {
        public override byte[] MakeRaw(MUSICMessage message)
        {
            var request = (SimulationTimeMessage)message;

            DataPdu disPdu = new DataPdu();
            disPdu.ExerciseID = (byte)request.MUSICHeader.ExerciseID;

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

            VariableDatum simTimeDatum = new VariableDatum();
            JObject simTime = new JObject();
            simTime["simTime"] = request.SimTime;
            AddStringToDatum(simTimeDatum, simTime.ToString());
            simTimeDatum.VariableDatumID = request.CommandIdentifier;
            disPdu.VariableDatums.Add(simTimeDatum);
            disPdu.NumberOfVariableDatumRecords = 1;

            DataOutputStream dos = new DataOutputStream(Endian.Big);
            disPdu.Marshal(dos);
            return dos.ConvertToBytes();
        }
    }
}
