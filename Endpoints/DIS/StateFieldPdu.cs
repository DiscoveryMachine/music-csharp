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

using MUSICLibrary.Endpoints.DIS.DIS_PDU_Makers;
using Newtonsoft.Json.Linq;
using OpenDis.Core;
using OpenDis.Dis1998;
using System.Linq;
using System.Text;

namespace MUSICLibrary.Endpoints.DIS
{
    public class StateFieldPdu : Pdu
    {
        public EntityID ConstructID { get; set; }
        //ushort padding
        //uint payload length
        public JObject Payload { get; internal set; }

        public StateFieldPdu()
        {
            ConstructID = new EntityID();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override int GetMarshalledSize()
        {
            int size = 0;

            size += base.GetMarshalledSize();
            size += ConstructID.GetMarshalledSize();
            size += 2; //padding
            size += 4; //payload length

            var stringBytes = Encoding.ASCII.GetBytes(Payload.ToString()).ToList();
            PduMaker.AddMissingPaddingTo(stringBytes);
            size += stringBytes.Count;

            return size;
        }

        public override void Marshal(DataOutputStream dos)
        {
            base.Marshal(dos);
            ConstructID.Marshal(dos);
            dos.WriteUnsignedShort(0);
            StringMarshaller.MarshalString_32bitLength(dos, Payload.ToString());
        }

        public override void Unmarshal(DataInputStream dis)
        {
            base.Unmarshal(dis);
            ConstructID.Unmarshal(dis);
            dis.ReadUnsignedShort();
            var lengthInBytes = dis.ReadInt() / 8;
            string payload = Encoding.ASCII.GetString(dis.ReadByteArray(lengthInBytes)).Replace("\0", "").Trim();
            Payload = JObject.Parse(payload);
        }
    }
}
