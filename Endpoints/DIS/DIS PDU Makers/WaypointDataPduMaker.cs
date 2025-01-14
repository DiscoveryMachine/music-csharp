﻿/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

//using MUSICLibrary.MUSIC_Messages;
//using static MUSICLibrary.Endpoints.DIS.DIS_PDU_Makers.DISPduStructs;

//namespace MUSICLibrary.Endpoints.DIS.DIS_PDU_Makers
//{
//    public class WaypointDataPduMaker : PduMaker
//    {
//        public override byte[] MakeRaw(MUSICMessage message)
//        {
//            WaypointDataMessage waypointData = (WaypointDataMessage)message;
//            DISPduStructs.WaypointRecord[] waypoints = new DISPduStructs.WaypointRecord[16];

//            for (int i = 0; i < waypoints.Length && i < waypointData.Waypoints.Count; i++)
//                waypoints[i] = ConvertWaypointRecord(waypointData.Waypoints[i]);

//            WaypointsPdu pdu = new WaypointsPdu()
//            {
//                header = MakePDUHeaderOfType(234, (byte)message.MUSICHeader.ExerciseID),
//                constructID = MakePDUEid(message.OriginID),
//                currentWaypointIndex = 0,
//                numberOfWaypoints = (ushort)waypointData.Waypoints.Count,
//                padding = 0,
//                startOrAppend = 1,
//                waypoints = waypoints,
//            };

//            return MarshalToByteArray(pdu);
//        }

//        private DISPduStructs.WaypointRecord ConvertWaypointRecord(MUSICLibrary.MUSIC_Messages.WaypointRecord waypoint)
//        {
//            return new DISPduStructs.WaypointRecord()
//            {
//                worldCoordinate = new EntityLocationRecord()
//                {
//                    x = waypoint.WorldCoordinateRecord.X,
//                    y = waypoint.WorldCoordinateRecord.Y,
//                    z = waypoint.WorldCoordinateRecord.Z,
//                },
//                estimatedArrivalTime = waypoint.EstimatedArrivalTime,
//                arrivalTimeErrorInSeconds = waypoint.ArrivalTimeError,
//            };
//        }
//    }
//}
