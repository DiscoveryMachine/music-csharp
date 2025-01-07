/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using MUSICLibrary.MUSIC_Messages;
using MUSICLibrary.MUSIC_Messages.Construct_Data;
using MUSICLibrary.MUSIC_Messages.Construct_Data.Dead_Reckoning;
using OpenDis.Core;
using OpenDis.Dis1998;
using System.Text;

namespace MUSICLibrary.Endpoints.DIS.Factories
{
    public class ConstructDataMessageFactory : IDISMUSICMessageFactory
    {
        private EntityIDRecord nullRecord = new EntityIDRecord(0, 0, 0);

        public MUSICMessage Create(byte[] pdu)
        {
            if (pdu[2] == 1) //offset of the pdu type. refactor into an enumeration alongside other offsets.
                return CreateFromLegacyEntityState(pdu);

            return CreateFromMUSICPdu(pdu);
        }

        private ConstructDataMessage CreateFromLegacyEntityState(byte[] pdu)
        {
            EntityStatePdu disPdu = new EntityStatePdu();
            disPdu.Unmarshal(new DataInputStream(pdu, Endian.Big));

            return new ConstructDataMessage
            {
                OriginID = disPdu.EntityID,
                MUSICHeader = new MUSICHeader(disPdu.ExerciseID),
                ConstructRender = ConstructRender.RenderedPhysical,
                ConstructType = ConstructType.Entity,
                Name = "Legacy",
                Physical = new PhysicalRecord
                {
                    ForceIDField = (Force)disPdu.ForceId,
                    Location = new MUSICVector3(disPdu.EntityLocation),
                    Orientation = new MUSICVector3(disPdu.EntityOrientation),
                    EntityType = new EntityTypeRecord(disPdu.EntityType),
                    LinearVelocity = new MUSICVector3(disPdu.EntityLinearVelocity),
                    DeadReckoningParameters = new DeadReckoningParametersRecord
                    (
                        disPdu.DeadReckoningParameters.DeadReckoningAlgorithm,
                        new MUSICVector3(disPdu.DeadReckoningParameters.EntityAngularVelocity),
                        new MUSICVector3(disPdu.DeadReckoningParameters.EntityLinearAcceleration)
                    ),
                },
                Callsign = Encoding.ASCII.GetString(disPdu.Marking.Characters).Replace("\0", "").Trim(),
            };
        }

        private ConstructDataMessage CreateFromMUSICPdu(byte[] pdu)
        {
            ConstructDataPdu musicPdu = new ConstructDataPdu();
            DataInputStream inStream = new DataInputStream(pdu, Endian.Big);
            musicPdu.Unmarshal(inStream);

            var constructData = new ConstructDataMessage
            {
                MUSICHeader = new MUSICHeader(musicPdu.ExerciseID),
                OriginID = musicPdu.ConstructID,
                PrimaryControllerID = musicPdu.PrimaryControllerID ?? musicPdu.ConstructID,
                CurrentControllerID = musicPdu.CurrentControllerID ?? musicPdu.ConstructID,
                Callsign = musicPdu.Callsign,
                ConstructRender = musicPdu.ConstructRender,
                ConstructType = musicPdu.ConstructType,
                Name = musicPdu.ConstructName,
                Interactions = musicPdu.Interactions,
            };

            if (musicPdu.ConstructRender == ConstructRender.GhostedConstruct ||
                musicPdu.ConstructRender == ConstructRender.GhostedLegacy)
            {
                constructData.GhostedID = musicPdu.GhostedID;
            }
            else if (musicPdu.ConstructRender == ConstructRender.RenderedPhysical ||
                musicPdu.ConstructRender == ConstructRender.UnrenderedPhysical)
            {
                constructData.Physical = new PhysicalRecord
                {
                    Damage = new DamageRecord
                    {
                        CatastrophicDamage = musicPdu.CatastrophicDamage,
                        MobilityDamage = musicPdu.MobilityDamage,
                        FirepowerDisabled = musicPdu.FirepowerDisabled,
                    },
                    DeadReckoningParameters = musicPdu.DeadReckoning,
                    EntityType = musicPdu.EntityTypeRecord,
                    ForceIDField = musicPdu.ForceID,
                    LinearVelocity = musicPdu.EntityLinearVelocity,
                    Location = musicPdu.EntityLocation,
                    Orientation = musicPdu.EntityOrientation,
                };
            }

            return constructData;
        }
    }
}
