/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using MUSICLibrary.MUSIC_Messages;
using System;
using System.Collections.Generic;

namespace MUSICLibrary.Endpoints.DIS.Factories
{
    public class WaypointDataMessageFactory : IDISMUSICMessageFactory
    {
        public MUSICMessage Create(byte[] pdu)
        {
            PduReader reader = new PduReader(pdu);
            var constructID = reader.ReadOriginID();
            //var startOrAppend = BitConverter.ToBoolean(pdu, PduReader.START_OR_APPEND_OFFSET); //TODO: Fix a potential problem with not storing startOrAppend in the waypoint message.
            var currentWaypointIndex = BitConverter.ToUInt16(pdu, PduReader.CURRENT_WAYPOINT_OFFSET);
            var numberOfWaypoints = BitConverter.ToUInt16(pdu, PduReader.NUMBER_OF_WAYPOINTS_OFFSET);

            var waypoints = new List<WaypointRecord>();

            int offset = PduReader.WAYPOINTS_OFFSET;

            for (int i = 0; i < numberOfWaypoints; i++)
                waypoints.Add(reader.ReadWaypointRecord(ref offset));

            return new WaypointDataMessage(reader.ReadExerciseID(), constructID, currentWaypointIndex, waypoints);
        }
    }
}
