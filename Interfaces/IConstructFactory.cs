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
using MUSICLibrary.MUSIC_Messages.Construct_Data;

namespace MUSICLibrary.Interfaces
{
    public interface IConstructFactory
    {
        IConstruct Create(ConstructDataMessage constructData);
        IConstruct Create(IConstructCreateInfo createInfo);
        bool IsConstructRegistered(string fullyQualifiedName);
        void RegisterLocalConstruct(IConstructCreateInfo createInfo);
        void RegisterRemoteConstruct(IConstructCreateInfo createInfo);
    }
}
