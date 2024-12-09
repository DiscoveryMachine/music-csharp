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
    public class DeadReckoningFPB : IDeadReckoningAlgorithm
    {
        public void ApplyDeadReckoning(PhysicalRecord physical, double deltaTime)
        {
            MUSICVector3 initialOrientation = (MUSICVector3)physical.Orientation.Clone();
            Rotation rotation = new Rotation(initialOrientation);

            Vector<double> velocityVector = Vector<double>.Build.Dense(3);
            velocityVector[0] = physical.LinearVelocity.X;
            velocityVector[1] = physical.LinearVelocity.Y;
            velocityVector[2] = physical.LinearVelocity.Z;

            physical.Location += rotation.ApplyInverseTo((MUSICVector3)(velocityVector * deltaTime));
        }

        public byte GetAlgorithmIndex()
        {
            return 6;
        }
    }
}