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
    public class DeadReckoningStatic : IDeadReckoningAlgorithm
    {
        public void ApplyDeadReckoning(PhysicalRecord record, double secondsToProject)
        {
        }

        public byte GetAlgorithmIndex()
        {
            return 1;
        }
    }
}