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
using MUSICLibrary.MUSIC_Messages.Construct_Data.Dead_Reckoning;
using MUSICLibrary.MUSIC_Messages.PerceptionData;
using OpenDis.Dis1998;
using System;
using System.Linq;
using System.Text;

namespace MUSICLibrary.Endpoints.DIS.Factories
{
    public class PduReader
    {
        private byte[] pdu;
        
        private const int EXERCISE_ID_OFFSET = 1;

        private const int ORIGIN_ID_OFFSET = 12;

        private const int PRIMARY_CONTROLLER_ID_OFFSET = 18;
        public const int CURRENT_CONTROLLER_ID_OFFSET = 24;
        public const int CONSTRUCT_RENDER_OFFSET = 30;
        public const int CONSTRUCT_TYPE_OFFSET = 31;
        public const int GHOSTED_ID_OFFSET = 32;

        public const int CONSTRUCT_DATA_FORCE_ID_OFFSET = 40;
        public const int CONSTRUCT_DATA_ENTITY_TYPE_OFFSET = 41;
        public const int CONSTRUCT_DATA_LOCATION_OFFSET = 49;
        public const int CONSTRUCT_DATA_ORIENTATION_OFFSET = 73;
        public const int VELOCITY_OFFSET = 85;
        public const int DEAD_RECKONING_OFFSET = 97;
        public const int DAMAGE_OFFSET = 137;
        public const int CONSTRUCT_DATA_STRING_SECTION_OFFSET = 160;

        public const int STATEFIELD_PAYLOAD_LENGTH_OFFSET = 20;
        public const int STATEFIELD_PAYLOAD_OFFSET = 24;

        public const int PERCEPTIONS_LENGTH_OFFSET = 22;
        public const int PERCEPTIONS_OFFSET = 24;

        public const int START_OR_APPEND_OFFSET = 18;
        public const int CURRENT_WAYPOINT_OFFSET = 20;
        public const int NUMBER_OF_WAYPOINTS_OFFSET = 22;
        public const int WAYPOINTS_OFFSET = 24;

        public PduReader(byte[] pdu)
        {
            this.pdu = pdu;
        }

        public EntityIDRecord ReadGhostedID()
        {
            return ReadEntityID(GHOSTED_ID_OFFSET);
        }

        public EntityIDRecord ReadCurrentControllerID()
        {
            return ReadEntityID(CURRENT_CONTROLLER_ID_OFFSET);
        }

        public EntityIDRecord ReadPrimaryControllerID()
        {
            return ReadEntityID(PRIMARY_CONTROLLER_ID_OFFSET);
        }

        public EntityIDRecord ReadOriginID()
        {
            return ReadEntityID(ORIGIN_ID_OFFSET);
        }

        public byte ReadPduType() // No unit test associated with it. Add one later.
        {
            return pdu[2];
        }

        public long ReadFirstVariableDatumID() // No unit test associated with it. Add one later.
        {
            if (pdu.Length < 44)
                return 0;

            var arr = new byte[4];
            Array.Copy(pdu, 40, arr, 0, 4);
            Array.Reverse(arr);
            return BitConverter.ToInt32(arr, 0);
        }

        public uint ReadExerciseID()
        {
            return pdu[EXERCISE_ID_OFFSET];
        }

        /// <summary>
        /// Reads a string from a PDU at the given offset by reading its length then reading the string in 8 byte chunks.
        /// This function increments the given offset by the number of bytes read.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public string ReadString(ref int i)
        {
            string result = "";

            var stringLengthInBits = BitConverter.ToUInt16(pdu, i);
            i += 2;
            
            for (int j = 0; j < stringLengthInBits; j += 8, i += 8)
            {
                byte[] stringBytes = pdu.Skip(i).Take(8).ToArray();
                result += Encoding.ASCII.GetString(stringBytes);
            }

            return result.Trim('\0');
        }

        public DamageRecord ReadDamage(int i)
        {
            double catastrophicDamage = BitConverter.ToDouble(pdu, i);
            double mobilityDamage = BitConverter.ToDouble(pdu, i + 8);
            bool firepowerDisabled = pdu[i + 16] > 0;
            return new DamageRecord(catastrophicDamage, mobilityDamage, firepowerDisabled);
        }

        public DeadReckoningParametersRecord ReadDeadReckoning(int i)
        {
            //Skipping the reserved space.

            var angularVelocity = ReadFloatVector3(i + 16); 
            var linearAcceleration = ReadFloatVector3(i + 28);
            return new DeadReckoningParametersRecord(pdu[i], linearAcceleration, angularVelocity);
        }

        public MUSICVector3 ReadFloatVector3(int i)
        {
            float x = BitConverter.ToSingle(pdu, i);
            float y = BitConverter.ToSingle(pdu, i + 4);
            float z = BitConverter.ToSingle(pdu, i + 8);
            return new MUSICVector3(x, y, z);
        }

        public MUSICVector3 ReadDoubleVector3(int i)
        {
            double x = BitConverter.ToDouble(pdu, i);
            double y = BitConverter.ToDouble(pdu, i + 8);
            double z = BitConverter.ToDouble(pdu, i + 16);
            return new MUSICVector3(x, y, z);
        }

        public EntityTypeRecord ReadEntityTypeRecord(int i)
        {
            var kind = (EntityKind)pdu[i];
            var domain = (EntityDomain)pdu[i + 1];
            var country = (EntityCountry)BitConverter.ToUInt16(pdu, i + 2);
            var category = pdu[i + 4];
            var subcategory = pdu[i + 5];
            var specific = pdu[i + 6];
            var extra = pdu[i + 7];

            return new EntityTypeRecord(kind, domain, country, category, subcategory, specific, extra);
        }

        private EntityIDRecord ReadEntityID(int i)
        {
            ushort site = BitConverter.ToUInt16(pdu, i);
            ushort app = BitConverter.ToUInt16(pdu, i + 2);
            ushort entity = BitConverter.ToUInt16(pdu, i + 4);
            return new EntityIDRecord(site, app, entity);
        }

        public PerceptionRecord ReadPerceptionRecord(ref int i)
        {
            var perceptionID = ReadEntityID(i);
            Force forceID = (Force)pdu[i + 6];
            var entityType = ReadEntityTypeRecord(i + 7);
            SensorType perceptionSystem = (SensorType)pdu[i + 8];
            uint accuracy = BitConverter.ToUInt32(pdu, i + 9);
            uint bearingError = BitConverter.ToUInt32(pdu, i + 13);
            uint altitudeError = BitConverter.ToUInt32(pdu, i + 17);
            uint rangeError = BitConverter.ToUInt32(pdu, i + 21);

            i += 32; //Perception records are 32 bytes in length.

            return new PerceptionRecord(perceptionID, forceID, entityType, perceptionSystem, accuracy,
                new PerceptionErrors(bearingError, altitudeError, rangeError));
        }

        public WaypointRecord ReadWaypointRecord(ref int i)
        {
            var worldCoordinate = ReadDoubleVector3(i);
            var estimatedArrivalTime = BitConverter.ToUInt32(pdu, i + 24);
            var arrivalTimeErrorInSeconds = BitConverter.ToUInt32(pdu, i + 28);

            i += 32; //Waypoint records are 32 bytes in length.

            return new WaypointRecord(worldCoordinate, estimatedArrivalTime, arrivalTimeErrorInSeconds);
        }

        public static string ReadStringFromDatum(VariableDatum datum)
        {
            string result = "";
            for (int i = 0; i < Math.Ceiling(datum.VariableDatumLength / 64f); i++)
                result += Encoding.ASCII.GetString(datum.VariableDatums[i].OtherParameters);
            return result.Replace('\0', ' ').TrimEnd(); //This trim is important to avoid a chain of null terminators at the end of a string.
        }
    }
}
