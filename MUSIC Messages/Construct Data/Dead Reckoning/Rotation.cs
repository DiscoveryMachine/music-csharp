//=====================================================================
// DISCOVERY MACHINE, INC. PROPRIETARY INFORMATION
//
// This software is supplied under the terms of a license agreement
// or nondisclosure agreement with Discovery Machine, Inc. and may
// not be copied or disclosed except in accordance with the terms of
// that agreement.
//
// Copyright 2022-23 Discovery Machine, Inc. All Rights Reserved.
//=====================================================================

// This class was originally Rotation.java from a library we used in Java to
// get the job done.  Unfortunately, the library did not exist in C# at the 
// time of writing this so we brought over the bare minimum required and made changes
// where necessary.  The original Rotation class can be found at the following link
// for a better understanding of what's going on here:
// https://commons.apache.org/proper/commons-math/javadocs/api-3.6.1/org/apache/commons/math3/geometry/euclidean/threed/package-summary.html

using System;
using System.Linq;

namespace MUSICLibrary.MUSIC_Messages.Construct_Data
{
    public class Rotation
    {
        /** Scalar coordinate of the quaternion. */
        private double q0;

        /** First coordinate of the vector part of the quaternion. */
        private double q1;

        /** Second coordinate of the vector part of the quaternion. */
        private double q2;

        /** Third coordinate of the vector part of the quaternion. */
        private double q3;

        // The following values are needed to determine rotation order and ultimately
        // set the initial orientation and should not change.
        private readonly MUSICVector3 PLUS_I = new MUSICVector3(1.0, 0.0, 0.0); 
        private readonly MUSICVector3 PLUS_J = new MUSICVector3(0.0, 1.0, 0.0); 
        private readonly MUSICVector3 PLUS_K = new MUSICVector3(0.0, 0.0, 1.0);

        public Rotation(double[,] m, double threshold)
        {
            if ((m.Length != 3) || (m.GetLength(0) != 3) || (m.GetLength(1) != 3) || m.GetLength(2) != 3)
            {
                Console.Out.WriteLine("The given matrix is not a 3x3 matrix");
                Console.Out.WriteLine(m.ToString());
            }

            double[,] oMatrix = OrthogonalizeMatrix(m, threshold);

            double det = oMatrix[0,0] * (oMatrix[1,1] * oMatrix[2,2] - oMatrix[2,1] * oMatrix[1,2]) -
                oMatrix[1,0] * (oMatrix[0,1] * oMatrix[2,2] - oMatrix[2,1] * oMatrix[0,2]) +
                oMatrix[2,0] * (oMatrix[0,1] * oMatrix[1,2] - oMatrix[1,1] * oMatrix[0,2]);

            if (det < 0.0)
            {
                Console.Out.WriteLine("The closest possible orthogonalized matrix has a negative determinant.");
            }

            double[] quat = ToQuaternion(oMatrix);
            q0 = quat[0];
            q1 = quat[1];
            q2 = quat[2];
            q3 = quat[3];
        }

        public Rotation(MUSICVector3 v) : this(v.X, v.Y, v.Z) { }

        public Rotation(double alpha1, double alpha2, double alpha3)
        {
            Rotation helper1 = new Rotation(PLUS_K, alpha1);
            Rotation helper2 = new Rotation(PLUS_J, alpha2);
            Rotation helper3 = new Rotation(PLUS_I, alpha3);
            Rotation composed = helper1.Compose(helper2.Compose(helper3, false), false);
            q0 = composed.q0;
            q1 = composed.q1;
            q2 = composed.q2;
            q3 = composed.q3;
        }

        public Rotation(MUSICVector3 axis, double angle)
        {
            double x = axis.X;
            double y = axis.Y;
            double z = axis.Z;

            double euclideanNorm = Math.Sqrt(x * x + y * y + z * z);
            if (euclideanNorm == 0)
            {
                Console.Out.WriteLine("The norm for this rotation axis is 0");
            }

            double coefficient = Math.Sin(+.5 * angle) / euclideanNorm;

            q0 = Math.Cos(+.5 * angle);
            q1 = coefficient * x;
            q2 = coefficient * y;
            q3 = coefficient * z;
        }
       

        public Rotation(double q0, double q1, double q2, double q3, bool normalize)
        {
            if (normalize)
            {
                double adjustment = 1.0 / Math.Sqrt(q0 * q0 + q1 * q1 + q2 * q2 + q3 * q3);
                q0 *= adjustment;
                q1 *= adjustment;
                q2 *= adjustment;
                q3 *= adjustment;
            }

            this.q0 = q0;
            this.q1 = q1;
            this.q2 = q2;
            this.q3 = q3;
        }

        private double[,] OrthogonalizeMatrix(double[,] m, double threshold)
        {
            double[] m0 = GetRow(m, 0);
            double[] m1 = GetRow(m, 1);
            double[] m2 = GetRow(m, 2);
            double x00 = m0[0];
            double x01 = m0[1];
            double x02 = m0[2];
            double x10 = m1[0];
            double x11 = m1[1];
            double x12 = m1[2];
            double x20 = m2[0];
            double x21 = m2[1];
            double x22 = m2[2];
            double fn = 0;
            double fn1;

            double[,] o = new double[3, 3];

            int counter = 0;

            // iterative correction: Xn+1 = Xn - 0.5 * (Xn.Mt.Xn - M)
            while (++counter < 11)
            {
                // Mt.Xn
                double mx00 = m0[0] * x00 + m1[0] * x10 + m2[0] * x20;
                double mx10 = m0[1] * x00 + m1[1] * x10 + m2[1] * x20;
                double mx20 = m0[2] * x00 + m1[2] * x10 + m2[2] * x20;
                double mx01 = m0[0] * x01 + m1[0] * x11 + m2[0] * x21;
                double mx11 = m0[1] * x01 + m1[1] * x11 + m2[1] * x21;
                double mx21 = m0[2] * x01 + m1[2] * x11 + m2[2] * x21;
                double mx02 = m0[0] * x02 + m1[0] * x12 + m2[0] * x22;
                double mx12 = m0[1] * x02 + m1[1] * x12 + m2[1] * x22;
                double mx22 = m0[2] * x02 + m1[2] * x12 + m2[2] * x22;

                // Xn+1
                o[0,0] = x00 - 0.5 * (x00 * mx00 + x01 * mx10 + x02 * mx20 - m0[0]);
                o[0,1] = x01 - 0.5 * (x00 * mx01 + x01 * mx11 + x02 * mx21 - m0[1]);
                o[0,2] = x02 - 0.5 * (x00 * mx02 + x01 * mx12 + x02 * mx22 - m0[2]);
                o[1,0] = x10 - 0.5 * (x10 * mx00 + x11 * mx10 + x12 * mx20 - m1[0]);
                o[1,1] = x11 - 0.5 * (x10 * mx01 + x11 * mx11 + x12 * mx21 - m1[1]);
                o[1,2] = x12 - 0.5 * (x10 * mx02 + x11 * mx12 + x12 * mx22 - m1[2]);
                o[2,0] = x20 - 0.5 * (x20 * mx00 + x21 * mx10 + x22 * mx20 - m2[0]);
                o[2,1] = x21 - 0.5 * (x20 * mx01 + x21 * mx11 + x22 * mx21 - m2[1]);
                o[2,2] = x22 - 0.5 * (x20 * mx02 + x21 * mx12 + x22 * mx22 - m2[2]);

                // correction on each element
                double corr00 = o[0,0] - m0[0];
                double corr01 = o[0,1] - m0[1];
                double corr02 = o[0,2] - m0[2];
                double corr10 = o[1,0] - m1[0];
                double corr11 = o[1,1] - m1[1];
                double corr12 = o[1,2] - m1[2];
                double corr20 = o[2,0] - m2[0];
                double corr21 = o[2,1] - m2[1];
                double corr22 = o[2,2] - m2[2];

                // Frobenius norm of the correction
                fn1 = corr00 * corr00 + corr01 * corr01 + corr02 * corr02 +
                    corr10 * corr10 + corr11 * corr11 + corr12 * corr12 +
                    corr20 * corr20 + corr21 * corr21 + corr22 * corr22;

                if (Math.Abs(fn1 - fn) <= threshold)
                    return o;

                // prepare for the next iteration.
                x00 = o[0,0];
                x01 = o[0,1];
                x02 = o[0,2];
                x10 = o[1,0];
                x11 = o[1,1];
                x12 = o[1,2];
                x20 = o[2,0];
                x21 = o[2,1];
                x22 = o[2,2];
                fn = fn1;
            }

            // Converge was not successful
            Console.Out.WriteLine("This matrix was not able to be orthogonalized.");
            return null;
        }

        private double[] GetRow(double[,] m, int rowNumber)
        {
            return Enumerable.Range(0, m.GetLength(1)).Select(x => m[rowNumber, x]).ToArray();
        }

        private static double[] ToQuaternion(double[,] orthongonalizedMatrix)
        {
            var ortho = orthongonalizedMatrix;
            double[] quaternion = new double[4];

            // There are different ways to compute the quaternions elements
            // from the matrix. They all involve computing one element from
            // the diagonal of the matrix, and computing the three other ones
            // using a formula involving a division by the first element,
            // which unfortunately can be zero. Since the norm of the
            // quaternion is 1, we know at least one element has an absolute
            // value greater or equal to 0.5, so it is always possible to
            // select the right formula and avoid division by zero and even
            // numerical inaccuracy. Checking the elements in turn and using
            // the first one greater than 0.45 is safe (this leads to a simple
            // test since qi = 0.45 implies 4 qi^2 - 1 = -0.19)

            double s = ortho[0,0] + ortho[1,1] + ortho[2,2];
            if (s > -0.19)
            {
                // compute q1 and deduce q0, q2 and q3
                quaternion[0] = 0.5 * Math.Sqrt(s + 1.0);
                double inv = 0.25 / quaternion[0];
                quaternion[1] = inv * (ortho[1,2] - ortho[2,1]);
                quaternion[2] = inv * (ortho[2,0] - ortho[0,2]);
                quaternion[3] = inv * (ortho[0,1] - ortho[1,0]);
            }
            else
            {
                s = ortho[0,0] - ortho[1,1] - ortho[2,2];
                if (s > -0.19)
                {
                    // compute q1 and deduce q0, q2 and q3
                    quaternion[1] = 0.5 * Math.Sqrt(s + 1.0);
                    double inv = 0.25 / quaternion[1];
                    quaternion[0] = inv * (ortho[1,2] - ortho[2,1]);
                    quaternion[2] = inv * (ortho[0,1] + ortho[1,0]);
                    quaternion[3] = inv * (ortho[0,2] + ortho[2,0]);
                }
                else
                {
                    s = ortho[1,1] - ortho[0,0] - ortho[2,2];
                    if (s > -0.19)
                    {
                        // compute q2 and deduce q0, q1 and q3
                        quaternion[2] = 0.5 * Math.Sqrt(s + 1.0);
                        double inv = 0.25 / quaternion[2];
                        quaternion[0] = inv * (ortho[2,0] - ortho[0,2]);
                        quaternion[1] = inv * (ortho[0,1] + ortho[1,0]);
                        quaternion[3] = inv * (ortho[2,1] + ortho[1,2]);
                    }
                    else
                    {
                        // compute q3 and deduce q0, q1 and q2
                        s = ortho[2,2] - ortho[0,0] - ortho[1,1];
                        quaternion[3] = 0.5 * Math.Sqrt(s + 1.0);
                        double inv = 0.25 / quaternion[3];
                        quaternion[0] = inv * (ortho[0,1] - ortho[1,0]);
                        quaternion[1] = inv * (ortho[0,2] + ortho[2,0]);
                        quaternion[2] = inv * (ortho[2,1] + ortho[1,2]);
                    }
                }
            }
            return quaternion;
        }

        public MUSICVector3 ApplyTo(MUSICVector3 mv3)
        {
            double x = mv3.X;
            double y = mv3.Y;
            double z = mv3.Z;

            double s = q1 * x + q2 * y + q3 * z;

            return new MUSICVector3(2 * (q0 * (x * q0 - (q2 * z - q3 * y)) + s * q1) - x,
                2 * (q0 * (y * q0 - (q3 * x - q1 * z)) + s * q2) - y,
                2 * (q0 * (z * q0 - (q1 * y - q2 * x)) + s * q3) - z);
        }

        public Rotation ApplyTo(Rotation helper)
        {
            return Compose(helper, true);
        }

        // true boolean represents Vector Operator, false is Frame Transform.
        public Rotation Compose(Rotation helper, bool isVectorOperator)
        {
            return isVectorOperator ? ComposeInternal(helper) : helper.ComposeInternal(this);
        }

        private Rotation ComposeInternal(Rotation h)
        {
            Rotation newHelper = new Rotation
                (h.q0 * q0 - (h.q1 * q1 + h.q2 * q2 + h.q3 * q3),
                h.q1 * q0 + h.q0 * q1 + (h.q2 * q3 - h.q3 * q2),
                h.q2 * q0 + h.q0 * q2 + (h.q3 * q1 - h.q1 * q3),
                h.q3 * q0 + h.q0 * q3 + (h.q1 * q2 - h.q2 * q1),
                false);

            return newHelper;
        }

        //Assumed rotation order of ?ZYX?
        public MUSICVector3 GetEulerAngles()
        {
            MUSICVector3 vector1 = ApplyTo(PLUS_K);
            MUSICVector3 vector2 = ApplyInverseTo(PLUS_I);
            
            if ((vector2.Z < -.9999999999) || (vector2.Z > .9999999999))
            {
                Console.Out.WriteLine("Cardan Euler Singularity is off.");
            }

            var eulerAngles = new MUSICVector3(
                Math.Atan2(vector2.Y, vector2.X),
                -Math.Asin(vector2.Z),
                Math.Atan2(vector1.Y, vector1.Z)
            );

            eulerAngles.X = double.IsNaN(eulerAngles.X) ? 0 : eulerAngles.X;
            eulerAngles.Y = double.IsNaN(eulerAngles.Y) ? 0 : eulerAngles.Y;
            eulerAngles.Z = double.IsNaN(eulerAngles.Z) ? 0 : eulerAngles.Z;

            return eulerAngles;
        }

        public MUSICVector3 ApplyInverseTo(MUSICVector3 v)
        {
            double x = v.X;
            double y = v.Y;
            double z = v.Z;

            double s = q1 * x + q2 * y + q3 * z;
            double m0 = -q0;

            return new MUSICVector3(2 * (m0 * (x * m0 - (q2 * z - q3 * y)) + s * q1) - x,
                2 * (m0 * (y * m0 - (q3 * x - q1 * z)) + s * q2) - y,
                2 * (m0 * (z * m0 - (q1 * y - q2 * x)) + s * q3) - z);
        }
    }
}
