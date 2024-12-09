//=====================================================================
// DISCOVERY MACHINE, INC. PROPRIETARY INFORMATION
//
// This software is supplied under the terms of a license agreement
// or nondisclosure agreement with Discovery Machine, Inc. and may
// not be copied or disclosed except in accordance with the terms  of
// that agreement.
//
// Copyright 2023 Discovery Machine, Inc. All Rights Reserved.
//=====================================================================

using System.Runtime.InteropServices;

namespace MUSICLibrary.Endpoints.DIS.DIS_PDU_Makers
{
    public static class DISPduStructs
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct WaypointsPdu //sz=32*numWaypoints+24
        {
            [MarshalAs(UnmanagedType.Struct)] public PDUHeader header;
            [MarshalAs(UnmanagedType.Struct)] public EntityIdentifier constructID; //12
            public byte startOrAppend; //18 //technically, this is 1 bit in length, but a byte is the smallest type available.
            public byte padding; //19
            public ushort currentWaypointIndex; //20
            public ushort numberOfWaypoints; //22
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)] public WaypointRecord[] waypoints; //24 (repeating. size=32, lensize = 34
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct WaypointRecord //sz=32
        {
            [MarshalAs(UnmanagedType.Struct)] public EntityLocationRecord worldCoordinate;
            public uint estimatedArrivalTime;
            public uint arrivalTimeErrorInSeconds;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct PerceptionDataPDU
        {
            [MarshalAs(UnmanagedType.Struct)] public PDUHeader header;
            [MarshalAs(UnmanagedType.Struct)] public EntityIdentifier constructID;
            public uint padding;
            public ushort perceptionRecordCount; //22
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)] public PerceptionRecord[] perceptionRecords; //24
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct PerceptionRecord //sz=32
        {
            [MarshalAs(UnmanagedType.Struct)] public EntityIdentifier perceptionID;
            public byte forceID;
            [MarshalAs(UnmanagedType.Struct)] public EntityTypeRecord entityType;
            public byte perceptionSystem;
            public float accuracy;
            public float bearingError;
            public float altitudeError;
            public float rangeError;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ConstructDataPDU
        {
            // i(inclusive) to n(exclusive)
            [MarshalAs(UnmanagedType.Struct)] public PDUHeader header; //0, 12
            [MarshalAs(UnmanagedType.Struct)] public EntityIdentifier constructID; //12, 18
            [MarshalAs(UnmanagedType.Struct)] public EntityIdentifier primaryController; //18, 24
            [MarshalAs(UnmanagedType.Struct)] public EntityIdentifier currentController; //24, 30
            [MarshalAs(UnmanagedType.Struct)] public ConstructInformationRecord constructInformationRecord; //30, 32
            [MarshalAs(UnmanagedType.Struct)] public EntityIdentifier ghostedID; //32, 38
            public ushort padding; //38, 40
            public byte forceID;  //40, 41

            [MarshalAs(UnmanagedType.Struct)] public EntityTypeRecord entityTypeRecord; //41, 49

            [MarshalAs(UnmanagedType.Struct)] public EntityLocationRecord entityLocationRecord; //49, 73
            [MarshalAs(UnmanagedType.Struct)] public EntityOrientationRecord entityOrientationRecord; //73, 85
            [MarshalAs(UnmanagedType.Struct)] public EntityLinearVelocityRecord entityLinearVelocityRecord; //85, 97
            [MarshalAs(UnmanagedType.Struct)] public DeadReckoningParametersRecord deadReckoningParametersRecord; //97, 137

            public double catastrophicDamage; //137, 145
            public double mobilityDamage; //145, 153
            public byte firepowerDisabled; //153, 154

            public uint padding2; //154, 158
            public ushort padding3; //158, 160

            public ushort constructCallsignLength; //160, 162
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)] public ulong[] callsignStringRecord; //162, N

            public ushort constructNameLength; //N, (N+2)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)] public ulong[] constructName; //(N+2), (N+2)+M

            public ushort interactionNameLength; //(N+2)+M, N+M+4
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)] public ulong[] interactionName; //N+M+4, N+M+X+4

            public ushort padding4; //N+M+X+4, N+M+X+6
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct DeadReckoningParametersRecord //sz=40
        {
            public byte deadReckoningAlgorithm;

            //Reserved space.
            public ulong deadReckoningOtherParameters1;
            public uint deadReckoningOtherParameters2;
            public ushort deadReckoningOtherParameters3;
            public byte deadReckoningOtherParameters4;

            [MarshalAs(UnmanagedType.Struct)] public EntityLinearAccelerationRecord entityLinearAccelerationRecord;
            [MarshalAs(UnmanagedType.Struct)] public EntityAngularVelocityRecord entityAngularVelocityRecord;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct EntityAngularVelocityRecord //sz=12
        {
            public float rateAboutX;
            public float rateAboutY;
            public float rateAboutZ;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct EntityLinearAccelerationRecord //sz=12
        {
            public float firstComponent;
            public float secondComponent;
            public float thirdComponent;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct EntityLinearVelocityRecord //sz=12
        {
            public float firstComponent;
            public float secondComponent;
            public float thirdComponent;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct EntityOrientationRecord //sz=12
        {
            public float psi;
            public float theta;
            public float phi;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct EntityLocationRecord //sz=24
        {
            public double x;
            public double y;
            public double z;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct EntityTypeRecord //sz=8
        {
            public byte kind;
            public byte domain;
            public ushort country;
            public byte category;
            public byte subcategory;
            public byte specific;
            public byte extra;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ConstructInformationRecord //sz=2
        {
            public byte constructRender;
            public byte constructType;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct StateFieldPDU //sz=28
        {
            [MarshalAs(UnmanagedType.Struct)] public PDUHeader header;
            [MarshalAs(UnmanagedType.Struct)] public EntityIdentifier constructID;
            public ushort padding;
            public uint payloadLength;
            //we need to define a size here for marshalling. The max # of payloads is up to (2^8)-1 payloads.
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)] public ulong[] payload;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PDUHeader //sz=12
        {
            public byte protocolVersion;
            public byte exerciseIdentifier;
            public byte pduType;
            public byte protocolFamily;
            public uint timestamp;
            public ushort pduLen;
            public ushort padding;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct EntityIdentifier //sz=6
        {
            [MarshalAs(UnmanagedType.Struct)] public SimulationAddressRecord simulationAddress;
            public ushort entityIdentity;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SimulationAddressRecord
        {
            public ushort site;
            public ushort app;
        }
    }
}
