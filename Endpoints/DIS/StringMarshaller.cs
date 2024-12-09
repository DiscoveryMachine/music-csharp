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
using OpenDis.Core;
using OpenDis.Dis1998;
using System.Linq;
using System.Text;

namespace MUSICLibrary.Endpoints.DIS
{
    public static class StringMarshaller
    {
        public static void MarshalString_16bitLength(DataOutputStream dos, string str)
        {
            var stringBytes = Encoding.ASCII.GetBytes(str).ToList();

            PduMaker.AddMissingPaddingTo(stringBytes);

            dos.WriteUnsignedShort((ushort)(stringBytes.Count * 8));
            foreach (byte b in stringBytes)
                dos.WriteUnsignedByte(b);
        }

        public static void MarshalString_32bitLength(DataOutputStream dos, string str)
        {
            var stringBytes = Encoding.ASCII.GetBytes(str).ToList();

            PduMaker.AddMissingPaddingTo(stringBytes);

            dos.WriteUnsignedInt((uint)(stringBytes.Count * 8));

            foreach (byte b in stringBytes)
                dos.WriteUnsignedByte(b);
        }

        public static string UnmarshalString(DataInputStream dis)
        {
            var length = dis.ReadUnsignedShort() / 8;
            return Encoding.ASCII.GetString(dis.ReadByteArray(length)).Replace("\0", "").Trim();
        }

        public static string UnmarshalStringFromDatum(DataInputStream dis)
        {
            var datum = new VariableDatum();
            datum.Unmarshal(dis);
            string result = "";
            foreach (var chunk in datum.VariableDatums)
                result += Encoding.ASCII.GetString(chunk.OtherParameters);
            return result.Replace("\0", "").Trim();
        }
    }
}
