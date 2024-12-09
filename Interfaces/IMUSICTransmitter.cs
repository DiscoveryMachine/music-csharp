//=====================================================================
// DISCOVERY MACHINE, INC. PROPRIETARY INFORMATION
//
// This software is supplied under the terms of a license agreement
// or nondisclosure agreement with Discovery Machine, Inc. and may
// not be copied or disclosed except in accordance with the terms  of
// that agreement.
//
// Copyright 2022 Discovery Machine, Inc. All Rights Reserved.
//=====================================================================

using MUSICLibrary.MUSIC_Messages;

namespace MUSICLibrary.Interfaces
{
    public interface IMUSICTransmitter
    {
        void Transmit(MUSICMessage message);

        uint GetExerciseID();
        SiteAndAppID GetSiteAndAppID();
    }
}
