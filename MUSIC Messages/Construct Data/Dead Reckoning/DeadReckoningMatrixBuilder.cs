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

using MathNet.Numerics.LinearAlgebra;
using System;

namespace MUSICLibrary.MUSIC_Messages.Construct_Data.Dead_Reckoning
{
    // This is where the implementation below originated from, but altered for readability.
    // https://github.com/open-dis/open-dis-java/tree/4345d53b151369eb5ced4d2167c641413e7775c0/src/main/java/edu/nps/moves/deadreckoning
    public class DeadReckoningMatrixBuilder
    {
        private const double MIN_ROTATION_RATE = 0.2 * Math.PI / 180;

        private readonly double deltaTime;
        private readonly double angularMagnitude;
        private readonly double sumOfSquares;
        private readonly double wDelta;

        private readonly PhysicalRecord physical;

        private readonly MUSICVector3 angularVelocity;

        private readonly Matrix<double> angularVelocityMatrix;
        private readonly Matrix<double> skewOmegaMatrix;
        private readonly Matrix<double> identityMatrix;

        private Matrix<double> angularVelocityMatrixScaled;
        private Matrix<double> identityMatrixScaled;
        private Matrix<double> skewMatrixScaled;

        public DeadReckoningMatrixBuilder(PhysicalRecord physical, double deltaTime)
        {
            this.physical = physical;
            this.deltaTime = deltaTime;

            angularVelocity = physical.DeadReckoningParameters.AngularVelocity;
            angularMagnitude = GetMagnitudeOf(angularVelocity);
            angularVelocityMatrix = GetAngularVelocityMatrixOf(angularVelocity); //The 'ww' matrix from the java DIS dead reckoning impl.
            sumOfSquares = GetSumOfSquares(angularVelocity);
            wDelta = angularMagnitude * deltaTime;
            skewOmegaMatrix = GetSkewMatrixOf(angularVelocity);
            identityMatrix = Matrix<double>.Build.DenseIdentity(3, 3);
        }

        public Matrix<double> BuildR1Matrix()
        {
            if (angularMagnitude < MIN_ROTATION_RATE)
                return Matrix<double>.Build.DenseOfMatrix(identityMatrix * deltaTime);

            double wwScalar = (wDelta - Math.Sin(wDelta)) / (sumOfSquares * angularMagnitude);
            double identityScalar = Math.Sin(wDelta) / angularMagnitude;
            double skewScalar = (1 - Math.Cos(wDelta)) / sumOfSquares;

            InitializeScaledMatricies(wwScalar, identityScalar, skewScalar);

            return angularVelocityMatrixScaled + identityMatrixScaled + skewMatrixScaled;
        }

        public Matrix<double> BuildR2Matrix()
        {
            if (angularMagnitude < MIN_ROTATION_RATE)
                return Matrix<double>.Build.DenseOfMatrix(identityMatrix * deltaTime * deltaTime * 0.5);

            double wwScalar = 0.5 * sumOfSquares * deltaTime * deltaTime;
            wwScalar -= Math.Cos(wDelta);
            wwScalar -= wDelta * Math.Sin(wDelta);
            wwScalar += 1;
            wwScalar /= sumOfSquares * sumOfSquares;

            double identityScalar = Math.Cos(wDelta) + wDelta * Math.Sin(wDelta) - 1;
            identityScalar /= sumOfSquares;

            double skewScalar = Math.Sin(wDelta) - wDelta * Math.Cos(wDelta);
            skewScalar /= sumOfSquares * angularMagnitude;

            InitializeScaledMatricies(wwScalar, identityScalar, skewScalar);

            return angularVelocityMatrixScaled + identityMatrixScaled + skewMatrixScaled;
        }

        public Matrix<double> BuildRotationMatrix()
        {
            if (angularMagnitude < MIN_ROTATION_RATE)
                return Matrix<double>.Build.DenseOfMatrix(identityMatrix);

            double identityScalar = Math.Cos(wDelta);
            double wwScalar = (1 - identityScalar) / GetSumOfSquares(angularVelocity);
            double skewScalar = Math.Sin(wDelta) / angularMagnitude;

            InitializeScaledMatricies(wwScalar, identityScalar, skewScalar);

            return angularVelocityMatrixScaled + identityMatrixScaled - skewMatrixScaled;
        }

        private void InitializeScaledMatricies(double wwScalar, double identityScalar, double skewScalar)
        {
            angularVelocityMatrixScaled = angularVelocityMatrix * wwScalar;
            identityMatrixScaled = identityMatrix * identityScalar;
            skewMatrixScaled = skewOmegaMatrix * skewScalar;
        }

        private Matrix<double> GetAngularVelocityMatrixOf(MUSICVector3 vector)
        {
            Matrix<double> angularVelocityMatrix = Matrix<double>.Build.Dense(3, 3);

            angularVelocityMatrix[0, 0] = vector.X * vector.X;
            angularVelocityMatrix[0, 1] = vector.X * vector.Y;
            angularVelocityMatrix[0, 2] = vector.X * vector.Z;
            angularVelocityMatrix[1, 0] = vector.Y * vector.X;
            angularVelocityMatrix[1, 1] = vector.Y * vector.Y;
            angularVelocityMatrix[1, 2] = vector.Y * vector.Z;
            angularVelocityMatrix[2, 0] = vector.Z * vector.X;
            angularVelocityMatrix[2, 1] = vector.Z * vector.Y;
            angularVelocityMatrix[2, 2] = vector.Z * vector.Z;

            return angularVelocityMatrix;
        }

        private Matrix<double> GetSkewMatrixOf(MUSICVector3 vector)
        {
            Matrix<double> skewMatrix = Matrix<double>.Build.Dense(3, 3);

            skewMatrix[0, 0] = 0;
            skewMatrix[0, 1] = -vector.Z;
            skewMatrix[0, 2] = vector.Y;
            skewMatrix[1, 0] = vector.Z;
            skewMatrix[1, 1] = 0;
            skewMatrix[1, 2] = -vector.X;
            skewMatrix[2, 0] = -vector.Y;
            skewMatrix[2, 1] = vector.X;
            skewMatrix[2, 2] = 0;

            return skewMatrix;
        }

        private double GetMagnitudeOf(MUSICVector3 vector)
        {
            return Math.Sqrt(GetSumOfSquares(vector));
        }

        private double GetSumOfSquares(MUSICVector3 vector)
        {
            return
                (vector.X * vector.X) +
                (vector.Y * vector.Y) +
                (vector.Z * vector.Z);
        }
    }
}
