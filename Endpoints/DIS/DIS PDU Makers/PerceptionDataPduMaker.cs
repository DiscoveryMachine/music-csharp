/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

//using MUSICLibrary.MUSIC_Messages;
//using static MUSICLibrary.Endpoints.DIS.DIS_PDU_Makers.DISPduStructs;

//namespace MUSICLibrary.Endpoints.DIS.DIS_PDU_Makers
//{
//    public class PerceptionDataPduMaker : PduMaker
//    {
//        public override byte[] MakeRaw(MUSICMessage message)
//        {
//            var perceptionData = (PerceptionDataMessage)message;
//            var header = MakePDUHeaderOfType(232, (byte)message.MUSICHeader.ExerciseID);

//            var constructID = MakePDUEid(perceptionData.OriginID);

//            var entityType = new DISPduStructs.EntityTypeRecord()
//            {
//                kind = 0,
//                domain = 5,
//                country = 2,
//                category = 2,
//                extra = 2,
//                specific = 2,
//                subcategory = 2
//            };

//            PerceptionRecord[] perceptions = new PerceptionRecord[16];

//            PerceptionRecord perception = new PerceptionRecord()
//            {
//                perceptionID = MakePDUEid(new EntityIDRecord(4, 4, 4)),
//                forceID = (byte)Force.Friendly,
//                entityType = entityType,
//                perceptionSystem = (byte)SensorType.ESM,
//                accuracy = 1.0f,
//                altitudeError = 0f,
//                bearingError = 0f,
//                rangeError = 0f,

//            };

//            perceptions[0] = perception;

//            PerceptionDataPDU pdu = new PerceptionDataPDU()
//            {
//                header = header,
//                constructID = constructID,
//                padding = 0,
//                perceptionRecordCount = 1,
//                perceptionRecords = perceptions,
//            };

//            return MarshalToByteArray(pdu);
//        }
//    }
//}
