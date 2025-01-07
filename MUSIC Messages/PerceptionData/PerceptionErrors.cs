/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using MUSICLibrary.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MUSICLibrary.MUSIC_Messages
{
    public class PerceptionErrors : IToJSON, IPrototype
    {
        public uint bearingError { get; private set; }
        public uint altitudeError { get; private set; }
        public uint rangeError { get; private set; }

        public const string BEARING_ERROR = "bearingError";
        public const string ALTITUDE_ERROR = "altitudeError";
        public const string RANGE_ERROR = "rangeError";

        [JsonConstructor]
        public PerceptionErrors(uint bearingError, uint altitudeError, uint rangeError)
        {
            this.bearingError = bearingError;
            this.altitudeError = altitudeError;
            this.rangeError = rangeError;
        }

        public PerceptionErrors(JObject jobj)
        {
            this.bearingError = jobj[BEARING_ERROR].ToObject<uint>();
            this.altitudeError = jobj[ALTITUDE_ERROR].ToObject<uint>();
            this.rangeError = jobj[RANGE_ERROR].ToObject<uint>();
        }

        public JObject ToJsonObject()
        {
            JObject jobj = new JObject();
            jobj.Add(BEARING_ERROR, bearingError);
            jobj.Add(ALTITUDE_ERROR, altitudeError);
            jobj.Add(RANGE_ERROR, rangeError);
            return jobj;
        }

        public override bool Equals(object obj)
        {
            if (obj is PerceptionErrors)
            {
                PerceptionErrors other = (PerceptionErrors)obj;

                if (
                    bearingError == other.bearingError &&
                    altitudeError == other.altitudeError &&
                    rangeError == other.rangeError
                    )
                    return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 1019388724;
            hashCode = hashCode * -1521134295 + bearingError.GetHashCode();
            hashCode = hashCode * -1521134295 + altitudeError.GetHashCode();
            hashCode = hashCode * -1521134295 + rangeError.GetHashCode();
            return hashCode;
        }

        public object Clone()
        {
            return new PerceptionErrors(bearingError, altitudeError, rangeError);
        }

        public static bool operator ==(PerceptionErrors pe1, PerceptionErrors pe2)
        {
            return EqualityComparer<PerceptionErrors>.Default.Equals(pe1, pe2);
        }

        public static bool operator !=(PerceptionErrors pe1, PerceptionErrors pe2)
        {
            return !(pe1 == pe2);
        }
    }
}
