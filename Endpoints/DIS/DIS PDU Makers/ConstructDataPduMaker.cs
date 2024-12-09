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
using MUSICLibrary.MUSIC_Messages.Construct_Data;
using MUSICLibrary.MUSIC_Messages.Construct_Data.Dead_Reckoning;
using OpenDis.Core;
using OpenDis.Dis1998;
using System;

namespace MUSICLibrary.Endpoints.DIS.DIS_PDU_Makers
{
    public class ConstructDataPduMaker : PduMaker
    {
        private readonly EntityIDRecord emptyRecord;

        public ConstructDataPduMaker()
        {
            emptyRecord = new EntityIDRecord(0, 0, 0);
        }

        public override byte[] MakeRaw(MUSICMessage message)
        {
            var constructData = (ConstructDataMessage)message;
            constructData.OriginID = constructData.OriginID ?? emptyRecord;
            constructData.PrimaryControllerID = constructData.PrimaryControllerID ?? constructData.OriginID;
            constructData.CurrentControllerID = constructData.CurrentControllerID ?? constructData.OriginID;

            ConstructDataPdu pdu = new ConstructDataPdu
            {
                ProtocolVersion = 6,
                ExerciseID = (byte)constructData.MUSICHeader.ExerciseID,
                PduType = 230,
                ProtocolFamily = 42,
                Timestamp = (uint)DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Padding = 0,
                ConstructID = new EntityID
                {
                    Site = (ushort)constructData.OriginID.SiteID,
                    Application = (ushort)constructData.OriginID.AppID,
                    Entity = (ushort)constructData.OriginID.EntityID,
                },
                PrimaryControllerID = new EntityID
                {
                    Site = (ushort)constructData.PrimaryControllerID.SiteID,
                    Application = (ushort)constructData.PrimaryControllerID.AppID,
                    Entity = (ushort)constructData.PrimaryControllerID.EntityID,
                },
                CurrentControllerID = new EntityID
                {
                    Site = (ushort)constructData.CurrentControllerID.SiteID,
                    Application = (ushort)constructData.CurrentControllerID.AppID,
                    Entity = (ushort)constructData.CurrentControllerID.EntityID,
                },
                ConstructRender = constructData.ConstructRender,
                ConstructType = constructData.ConstructType,

                Callsign = constructData.Callsign,
                ConstructName = constructData.Name,
                Interactions = constructData.Interactions ?? new InteractionRecord(),
            };

            if (constructData.GhostedID != null)
            {
                pdu.GhostedID = new EntityID
                {
                    Site = (ushort)constructData.GhostedID.SiteID,
                    Application = (ushort)constructData.GhostedID.AppID,
                    Entity = (ushort)constructData.GhostedID.EntityID,
                };
            }

            if (constructData.Physical != null)
            {
                var physical = constructData.Physical;
                pdu.ForceID = physical.ForceIDField;
                pdu.EntityTypeRecord = new EntityTypeRecord
                {
                    Kind = physical.EntityType.Kind,
                    Domain = physical.EntityType.Domain,
                    Country = physical.EntityType.Country,
                    Category = physical.EntityType.Category,
                    Subcategory = physical.EntityType.Subcategory,
                    Specific = physical.EntityType.Specific,
                    Extra = physical.EntityType.Extra,
                };
                pdu.EntityLocation = physical.Location;
                pdu.EntityOrientation = physical.Orientation;
                pdu.EntityLinearVelocity = physical.LinearVelocity;

                var deadReckoning = physical.DeadReckoningParameters;
                pdu.DeadReckoning = new DeadReckoningParametersRecord
                {
                    Algorithm = deadReckoning.Algorithm,
                    LinearAcceleration = deadReckoning.LinearAcceleration,
                    AngularVelocity = deadReckoning.AngularVelocity,
                };

                var damage = physical.Damage;
                pdu.CatastrophicDamage = damage.CatastrophicDamage;
                pdu.MobilityDamage = damage.MobilityDamage;
                pdu.FirepowerDisabled = damage.FirepowerDisabled;
            }

            pdu.Length = (ushort)pdu.GetMarshalledSize();

            DataOutputStream dos = new DataOutputStream(Endian.Big);
            pdu.Marshal(dos);
            return dos.ConvertToBytes();
        }
    }
}