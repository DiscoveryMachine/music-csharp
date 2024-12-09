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

using System;
using MUSICLibrary.Endpoints.DIS.DIS_PDU_Makers;
using MUSICLibrary.MUSIC_Messages;
using MUSICLibrary.MUSIC_Messages.Construct_Data.Dead_Reckoning;
using OpenDis.Core;
using OpenDis.Dis1998;

namespace MUSICLibrary.Endpoints.DIS
{
    public class ConstructDataPdu : Pdu
    {
        public EntityID ConstructID { get; set; }
        public EntityID PrimaryControllerID { get; set; }
        public EntityID CurrentControllerID { get; set; }
        public ConstructRender ConstructRender { get; set; }
        public ConstructType ConstructType { get; set; }
        public EntityID GhostedID { get; set; }
        public Force ForceID { get; set; }
        public EntityTypeRecord EntityTypeRecord { get; set; }
        public MUSICVector3 EntityLocation { get; set; }
        public MUSICVector3 EntityOrientation { get; set; }
        public MUSICVector3 EntityLinearVelocity { get; set; }
        public DeadReckoningParametersRecord DeadReckoning { get; set; }
        public double CatastrophicDamage { get; set; }
        public double MobilityDamage { get; set; }
        public bool FirepowerDisabled { get; set; }
        public string Callsign { get; set; }
        public string ConstructName { get; set; }
        public InteractionRecord Interactions { get; set; }

        public override void Marshal(DataOutputStream dos)
        {
            base.Marshal(dos);
            ConstructID.Marshal(dos);

            PrimaryControllerID.Marshal(dos);
            CurrentControllerID.Marshal(dos);

            dos.WriteByte((byte)ConstructRender);
            dos.WriteByte((byte)ConstructType);

            if (ConstructRender == ConstructRender.GhostedConstruct || 
                ConstructRender == ConstructRender.GhostedLegacy)
            {
                GhostedID.Marshal(dos);
                dos.WriteUnsignedShort(0);
            }

            if (ConstructRender == ConstructRender.UnrenderedPhysical ||
                ConstructRender == ConstructRender.RenderedPhysical)
            {
                dos.WriteByte((byte)ForceID);
                MarshalEntityTypeRecord(dos);
                MarshalVector3Double(dos, EntityLocation);
                MarshalVector3Float(dos, EntityOrientation);
                MarshalVector3Float(dos, EntityLinearVelocity);
                MarshalDeadReckoning(dos);
                dos.WriteFloat((float)CatastrophicDamage);
                dos.WriteFloat((float)MobilityDamage);
                dos.WriteUnsignedByte(Convert.ToByte(FirepowerDisabled));
                dos.WriteUnsignedInt(0);
                dos.WriteUnsignedShort(0);
            }

            StringMarshaller.MarshalString_16bitLength(dos, Callsign);
            StringMarshaller.MarshalString_16bitLength(dos, ConstructName);
            StringMarshaller.MarshalString_16bitLength(dos, Interactions.ToCsvString());
            dos.WriteUnsignedShort(0);
        }

        public override int GetMarshalledSize()
        {
            int size = 0;

            size += base.GetMarshalledSize();
            size += ConstructID.GetMarshalledSize();
            size += PrimaryControllerID.GetMarshalledSize();
            size += CurrentControllerID.GetMarshalledSize();
            size += 1; //render
            size += 1; //type

            if (ConstructRender == ConstructRender.GhostedConstruct ||
                ConstructRender == ConstructRender.GhostedLegacy)
            {
                size += GhostedID?.GetMarshalledSize() ?? 0;
                size += 2; //padding
            }
            else if (ConstructRender == ConstructRender.UnrenderedPhysical ||
                ConstructRender == ConstructRender.RenderedPhysical)
            {
                size += 1; //force
                size += 8; //entity type
                size += 24; //location
                size += 12; //orientation
                size += 12; //linear velocity
                size += 40; //dead reckoning
                size += 8; //catastrophic damage
                size += 8; //mobility damage
                size += 1; //firepower disabled
                size += 6; //padding
            }

            //TODO: Refactor below.

            size += 2; //callsign length
            VariableDatum callsignDatum = new VariableDatum();
            PduMaker.AddStringToDatum(callsignDatum, Callsign);
            size += callsignDatum.GetMarshalledSize(); //StringMarshaller.CreateVariableDatumFrom(Callsign).GetMarshalledSize();

            size += 2; //construct name length
            VariableDatum nameDatum = new VariableDatum();
            PduMaker.AddStringToDatum(nameDatum, ConstructName);
            size += nameDatum.GetMarshalledSize();

            size += 2; //interactions length.
            VariableDatum interactionsDatum = new VariableDatum();
            PduMaker.AddStringToDatum(interactionsDatum, Interactions.ToCsvString());
            size += interactionsDatum.GetMarshalledSize();

            size += 2; //padding

            return size;
        }

        private void MarshalVector3Double(DataOutputStream dos, MUSICVector3 vec)
        {
            dos.WriteDouble(vec.X);
            dos.WriteDouble(vec.Y);
            dos.WriteDouble(vec.Z);
        }

        private MUSICVector3 UnmarshalVector3Double(DataInputStream dis)
        {
            return new MUSICVector3
            {
                X = dis.ReadDouble(),
                Y = dis.ReadDouble(),
                Z = dis.ReadDouble(),
            };
        }

        private void MarshalVector3Float(DataOutputStream dos, MUSICVector3 vec)
        {
            dos.WriteFloat((float)vec.X);
            dos.WriteFloat((float)vec.Y);
            dos.WriteFloat((float)vec.Z);
        }

        private MUSICVector3 UnmarshalVector3Float(DataInputStream dis)
        {
            return new MUSICVector3
            {
                X = dis.ReadFloat(),
                Y = dis.ReadFloat(),
                Z = dis.ReadFloat(),
            };
        }

        private void MarshalDeadReckoning(DataOutputStream dos)
        {
            dos.WriteByte(DeadReckoning.Algorithm.GetAlgorithmIndex());
            dos.WriteUnsignedLong(0);
            dos.WriteUnsignedInt(0);
            dos.WriteUnsignedShort(0);
            dos.WriteUnsignedByte(0);
            MarshalVector3Float(dos, DeadReckoning.LinearAcceleration);
            MarshalVector3Float(dos, DeadReckoning.AngularVelocity);
        }

        private void UnmarshalDeadReckoning(DataInputStream dis)
        {
            var algorithm = dis.ReadByte();
            dis.ReadLong();
            dis.ReadInt();
            dis.ReadShort();
            dis.ReadByte();
            var linearAcceleration = UnmarshalVector3Float(dis);
            var angularVelocity = UnmarshalVector3Float(dis);
            DeadReckoning = new DeadReckoningParametersRecord(algorithm, angularVelocity, linearAcceleration);
        }

        private void MarshalEntityTypeRecord(DataOutputStream dos)
        {
            dos.WriteByte((byte)EntityTypeRecord.Kind);
            dos.WriteByte((byte)EntityTypeRecord.Domain);
            dos.WriteUnsignedShort((ushort)EntityTypeRecord.Country);
            dos.WriteByte(EntityTypeRecord.Category);
            dos.WriteByte(EntityTypeRecord.Subcategory);
            dos.WriteByte(EntityTypeRecord.Specific);
            dos.WriteByte(EntityTypeRecord.Extra);
        }

        private void UnmarshalEntityTypeRecord(DataInputStream dis)
        {
            EntityTypeRecord = new EntityTypeRecord
            {
                Kind = (EntityKind)dis.ReadByte(),
                Domain = (EntityDomain)dis.ReadByte(),
                Country = (EntityCountry)dis.ReadUnsignedShort(),
                Category = dis.ReadByte(),
                Subcategory = dis.ReadByte(),
                Specific = dis.ReadByte(),
                Extra = dis.ReadByte(),
            };
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override void Unmarshal(DataInputStream dis)
        {
            base.Unmarshal(dis);

            ConstructID = new EntityID();
            ConstructID.Unmarshal(dis);

            PrimaryControllerID = new EntityID();
            PrimaryControllerID.Unmarshal(dis);

            CurrentControllerID = new EntityID();
            CurrentControllerID.Unmarshal(dis);

            ConstructRender = (ConstructRender)dis.ReadByte();
            ConstructType = (ConstructType)dis.ReadByte();

            if (ConstructRender == ConstructRender.GhostedConstruct ||
                ConstructRender == ConstructRender.GhostedLegacy)
            { 
                GhostedID = new EntityID();
                GhostedID.Unmarshal(dis);
                dis.ReadUnsignedShort();
            }
            else if (ConstructRender == ConstructRender.UnrenderedPhysical || 
                ConstructRender == ConstructRender.RenderedPhysical)
            {
                ForceID = (Force)dis.ReadByte();
                UnmarshalEntityTypeRecord(dis);

                EntityLocation = UnmarshalVector3Double(dis);
                EntityOrientation = UnmarshalVector3Float(dis);
                EntityLinearVelocity = UnmarshalVector3Float(dis);

                UnmarshalDeadReckoning(dis);

                CatastrophicDamage = dis.ReadFloat();
                MobilityDamage = dis.ReadFloat();
                FirepowerDisabled = Convert.ToBoolean(dis.ReadByte());
                dis.ReadInt();
                dis.ReadShort();
            }

            Callsign = StringMarshaller.UnmarshalString(dis);
            ConstructName = StringMarshaller.UnmarshalString(dis);
            Interactions = new InteractionRecord(StringMarshaller.UnmarshalString(dis));

            dis.ReadUnsignedShort();
        }
    }
}
