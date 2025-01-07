/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using MUSICLibrary.Interfaces;
using MathNet.Numerics.LinearAlgebra;
using System;
using OpenDis.Dis1998;

namespace MUSICLibrary.MUSIC_Messages
{
    public class MUSICVector3 : IToJSON, IPrototype
    {
        private const string VECTOR_X = "x";
        private const string VECTOR_Y = "y";
        private const string VECTOR_Z = "z";

        private const string VECTOR_PSI = "psi";
        private const string VECTOR_THETA = "theta";
        private const string VECTOR_PHI = "phi";

        [JsonProperty(VECTOR_X)]
        public double X { get; set; }

        [JsonProperty(VECTOR_Y)]
        public double Y { get; set; }

        [JsonProperty(VECTOR_Z)]
        public double Z { get; set; }

        [JsonProperty(VECTOR_PSI)]
        private double Psi { set { X = value;  } }

        [JsonProperty(VECTOR_THETA)]
        private double Theta { set { Y = value;  } }

        [JsonProperty(VECTOR_PHI)]
        private double Phi { set { Z = value;  } }

        public MUSICVector3() { }

        public MUSICVector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static explicit operator MUSICVector3(Vector<double> v)
        {
            return new MUSICVector3(v[0], v[1], v[2]);
        }

        public static explicit operator Vector<double>(MUSICVector3 v)
        {
            var result = Vector<double>.Build.Dense(3);
            result[0] = v.X;
            result[1] = v.Y;
            result[2] = v.Z;
            return result;
        }

        public MUSICVector3(JObject json)
        {
            if (json is null)
                return;

            if (json.ContainsKey(VECTOR_X) && json.ContainsKey(VECTOR_Y) && json.ContainsKey(VECTOR_Z))
            {
                X = (double)json[VECTOR_X];
                Y = (double)json[VECTOR_Y];
                Z = (double)json[VECTOR_Z];
            }
            else
            {
                X = (double)json[VECTOR_PSI];
                Y = (double)json[VECTOR_THETA];
                Z = (double)json[VECTOR_PHI];
            }
        }

        public MUSICVector3(Vector3Double entityLocation)
        {
            X = entityLocation.X;
            Y = entityLocation.Y;
            Z = entityLocation.Z;
        }

        public MUSICVector3(Orientation entityOrientation)
        {
            X = entityOrientation.Psi;
            Y = entityOrientation.Theta;
            Z = entityOrientation.Phi;
        }

        public MUSICVector3(Vector3Float entityLinearVelocity)
        {
            X = entityLinearVelocity.X;
            Y = entityLinearVelocity.Y;
            Z = entityLinearVelocity.Z;
        }

        public JObject ToJsonObject()
        {
            JObject jobj = new JObject();

            jobj.Add(VECTOR_X, X);
            jobj.Add(VECTOR_Y, Y);
            jobj.Add(VECTOR_Z, Z);

            return jobj;
        }

        public JObject ToOrientationObject()
        {
            JObject obj = new JObject();

            obj.Add(VECTOR_PSI, X);
            obj.Add(VECTOR_THETA, Y);
            obj.Add(VECTOR_PHI, Z);

            return obj;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;

            if(obj is MUSICVector3)
            {
                MUSICVector3 other = (MUSICVector3)obj;

                if(X == other.X && Y == other.Y && Z == other.Z)
                {
                    return true;
                }
            }

            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 373119288;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Z.GetHashCode();
            return hashCode;
        }

        public object Clone()
        {
            return new MUSICVector3(X, Y, Z);
        }

        public static bool operator == (MUSICVector3 v1, MUSICVector3 v2)
        {
            if (v1 is null && v2 is null) return true;
            if (!(v1 is null))
                return v1.Equals(v2);
            else return false;
        }

        public static bool operator != (MUSICVector3 v1, MUSICVector3 v2)
        {
            return !(v1 == v2);
        }

        public static MUSICVector3 operator + (MUSICVector3 v1, MUSICVector3 v2)
        {
            return new MUSICVector3(
                    v1.X + v2.X,
                    v1.Y + v2.Y,
                    v1.Z + v2.Z
                );
        }

        public static MUSICVector3 operator *(MUSICVector3 v1, double scalar)
        {
            return new MUSICVector3(
                    v1.X * scalar,
                    v1.Y * scalar,
                    v1.Z * scalar
                );
        }
    }
}
