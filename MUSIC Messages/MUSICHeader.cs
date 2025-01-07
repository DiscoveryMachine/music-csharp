/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using MUSICLibrary.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace MUSICLibrary.MUSIC_Messages
{
    public class MUSICHeader : IToJSON
    {
        public uint ExerciseID { get; set; }
        public ulong Timestamp { get; set; }

        public MUSICHeader(uint exerciseID)
        {
            ExerciseID = exerciseID;
            Timestamp = (ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        [JsonConstructor]
        public MUSICHeader(uint exerciseID, ulong timestamp)
        {
            ExerciseID = exerciseID;
            Timestamp = timestamp;
        }

        public JObject ToJsonObject()
        {
            JObject json = new JObject();
            json["exerciseID"] = ExerciseID;
            json["timestamp"] = Timestamp;
            return json;
        }

        public override int GetHashCode()
        {
            return -2000878970 + ExerciseID.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var header = obj as MUSICHeader;
            return header != null &&
                   ExerciseID == header.ExerciseID;
        }

        public static bool operator ==(MUSICHeader header1, MUSICHeader header2)
        {
            return EqualityComparer<MUSICHeader>.Default.Equals(header1, header2);
        }

        public static bool operator !=(MUSICHeader header1, MUSICHeader header2)
        {
            return !(header1 == header2);
        }
    }
}
