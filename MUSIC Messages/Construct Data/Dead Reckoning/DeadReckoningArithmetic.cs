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

namespace MUSICLibrary.MUSIC_Messages.Construct_Data.Dead_Reckoning
{
    // This is where the implementation below originated from, but altered for readability.
    // https://github.com/open-dis/open-dis-java/tree/4345d53b151369eb5ced4d2167c641413e7775c0/src/main/java/edu/nps/moves/deadreckoning
    public static class DeadReckoningArithmetic
    {
        public static MUSICVector3 CalculateLinearMotion(MUSICVector3 v0, MUSICVector3 v1, double deltaTime)
        {
           return new MUSICVector3(
                v0.X + v1.X * deltaTime,
                v0.Y + v1.Y * deltaTime,
                v0.Z + v1.Z * deltaTime
            );
        }

        public static MUSICVector3 CalculateUpdatedOrientation(PhysicalRecord physical, double deltaTime)
        {
            MUSICVector3 initialOrientation = (MUSICVector3)physical.Orientation.Clone();

            Matrix<double> deadReckoningMatrix = new DeadReckoningMatrixBuilder(physical, deltaTime).BuildRotationMatrix();

            Rotation updatedOrientation = new Rotation(deadReckoningMatrix.ToArray(), 1e-15).ApplyTo(new Rotation(initialOrientation));

            return updatedOrientation.GetEulerAngles();
        }

        public static MUSICVector3 CalculateLinearMotionWithAcceleration(PhysicalRecord physical, double deltaTime)
        {
            var result = CalculateLinearMotion(physical.Location, physical.LinearVelocity, deltaTime);

            var linearAcceleration = physical.DeadReckoningParameters.LinearAcceleration;
            result.X += 0.5 * linearAcceleration.X * deltaTime * deltaTime;
            result.Y += 0.5 * linearAcceleration.Y * deltaTime * deltaTime;
            result.Z += 0.5 * linearAcceleration.Z * deltaTime * deltaTime;

            return result;
        }
    }
}
