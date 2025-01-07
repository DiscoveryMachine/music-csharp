/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using MUSICLibrary.MUSIC_Messages;
using OpenDis.Dis1998;
using System;
using System.Collections.Generic;
using System.Text;

namespace MUSICLibrary.Endpoints.DIS.DIS_PDU_Makers
{
    public abstract class PduMaker //TODO: These functions need to be organized better. 
    {
        public PduMaker() { }

        public abstract byte[] MakeRaw(MUSICMessage message);

        public static void AddStringToDatum(VariableDatum datum, string str)
        {
            foreach (ulong nameChunk in ConvertStringToULongArray(str))
            {
                EightByteChunk chunk = new EightByteChunk();
                chunk.OtherParameters = BitConverter.GetBytes(nameChunk);
                datum.VariableDatums.Add(chunk);
                datum.VariableDatumLength += 64;
            }
        }

        public static string GetStringFromDatum(VariableDatum datum)
        {
            string result = "";
            foreach (var chunk in datum.VariableDatums)
                result += Encoding.ASCII.GetString(chunk.OtherParameters);
            return result.Replace("\0", "").Trim();
        }

        private static ulong[] ConvertStringToULongArray(string str)
        {
            List<ulong> result = new List<ulong>();

            List<byte> stringAsBytes = new List<byte>(Encoding.ASCII.GetBytes(str));
            AddMissingPaddingTo(stringAsBytes);

            for (int i = 0; i < stringAsBytes.Count; i += 8)
                result.Add(BitConverter.ToUInt64(stringAsBytes.ToArray(), i));

            return result.ToArray();
        }

        public static void AddMissingPaddingTo(List<byte> array)
        {
            int missingPadding = 8 - (array.Count % 8);
            if (missingPadding != 8)
                for (int i = 0; i < missingPadding; i++)
                    array.Add(0);
        }
    }
}
