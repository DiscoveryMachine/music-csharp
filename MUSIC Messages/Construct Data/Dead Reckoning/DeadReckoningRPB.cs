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
    public class DeadReckoningRPB : IDeadReckoningAlgorithm
    {
        public void ApplyDeadReckoning(PhysicalRecord physical, double deltaTime)
        {
            UpdateLocation(physical, deltaTime);
            physical.Orientation = DeadReckoningArithmetic.CalculateUpdatedOrientation(physical, deltaTime);
        }

        private void UpdateLocation(PhysicalRecord physical, double deltaTime)
        {
            Vector<double> linearVelocity = (Vector<double>)(MUSICVector3)physical.LinearVelocity.Clone();
            Rotation currentOrientation = new Rotation(physical.Orientation);

            Matrix<double> r1Matrix = new DeadReckoningMatrixBuilder(physical, deltaTime).BuildR1Matrix();

            physical.Location += currentOrientation.ApplyInverseTo((MUSICVector3)(r1Matrix * linearVelocity));
        }

        public byte GetAlgorithmIndex()
        {
            return 7;
        }
    }
}