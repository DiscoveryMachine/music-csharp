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

namespace MUSICLibrary.MUSIC_Messages.Construct_Data.Dead_Reckoning
{
    // This is where the implementation below originated from, but altered for readability.
    // https://github.com/open-dis/open-dis-java/tree/4345d53b151369eb5ced4d2167c641413e7775c0/src/main/java/edu/nps/moves/deadreckoning
    public class DeadReckoningFVB : IDeadReckoningAlgorithm
    {
        public void ApplyDeadReckoning(PhysicalRecord physical, double deltaTime)
        {
            MUSICVector3 linearVelocity = (MUSICVector3)physical.LinearVelocity.Clone();
            MUSICVector3 linearAcceleration = (MUSICVector3)physical.DeadReckoningParameters.LinearAcceleration.Clone();
            Rotation orientation = new Rotation(physical.Orientation);
            MUSICVector3 updatedLocation = orientation.ApplyInverseTo((linearVelocity + (linearAcceleration * 0.5 * deltaTime)) * deltaTime);
            physical.Location += updatedLocation;

            physical.LinearVelocity = DeadReckoningArithmetic.CalculateLinearMotion(linearVelocity, linearAcceleration, deltaTime);
        }

        public byte GetAlgorithmIndex()
        {
            return 9;
        }
    }
}