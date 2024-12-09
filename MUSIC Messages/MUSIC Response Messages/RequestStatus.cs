﻿//=====================================================================
//DISCOVERY MACHINE, INC. PROPRIETARY INFORMATION
//
//This software is supplied under the terms of a license agreement
//or nondisclosure agreement with Discovery Machine, Inc. and may
//not be copied or disclosed except in accordance with the terms  of
//that agreement.
//
//Copyright 2022 Discovery Machine, Inc. All Rights Reserved.
//=====================================================================  

namespace MUSICLibrary.MUSIC_Messages.MUSIC_Response_Messages
{
    public enum RequestStatus
    {
        Other,
        Pending,
        Executing,
        PartiallyComplete,
        Complete,
        Aborted,
        Paused
    }
}
