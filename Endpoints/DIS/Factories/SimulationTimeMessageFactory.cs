/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using MUSICLibrary.Endpoints.DIS.DIS_PDU_Makers;
using MUSICLibrary.MUSIC_Messages;
using MUSICLibrary.MUSIC_Messages.Targeted_MUSIC_Messages;
using Newtonsoft.Json.Linq;
using OpenDis.Core;
using OpenDis.Dis1998;

namespace MUSICLibrary.Endpoints.DIS.Factories
{
    public class SimulationTimeMessageFactory : IDISMUSICMessageFactory
    {
        public MUSICMessage Create(byte[] pdu)
        {
            DataPdu disPdu = new DataPdu();
            disPdu.Unmarshal(new DataInputStream(pdu, Endian.Big));

            JObject simTimeObj = JObject.Parse(PduMaker.GetStringFromDatum(disPdu.VariableDatums[0]));

            return new SimulationTimeMessage
            {
                MUSICHeader = new MUSICHeader(disPdu.ExerciseID),
                OriginID = disPdu.OriginatingEntityID,
                ReceiverID = disPdu.ReceivingEntityID,
                SimTime = simTimeObj["simTime"].ToObject<uint>(),
            };
        }
    }
}
