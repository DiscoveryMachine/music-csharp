//=====================================================================
//DISCOVERY MACHINE, INC. PROPRIETARY INFORMATION
//
//This software is supplied under the terms of a license agreement
//or nondisclosure agreement with Discovery Machine, Inc. and may
//not be copied or disclosed except in accordance with the terms of
//that agreement.
//
//Copyright 2022-23 Discovery Machine, Inc. All Rights Reserved.
//=====================================================================

namespace MUSICLibrary.MUSIC_Messages.Construct_Data.Dead_Reckoning
{
    public interface IDeadReckoningAlgorithm
    {
        void ApplyDeadReckoning(PhysicalRecord physical, double deltaTime);

        byte GetAlgorithmIndex();
    }
}
