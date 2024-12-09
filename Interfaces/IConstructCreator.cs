//=====================================================================
// DISCOVERY MACHINE, INC. PROPRIETARY INFORMATION
//
// This software is supplied under the terms of a license agreement
// or nondisclosure agreement with Discovery Machine, Inc. and may
// not be copied or disclosed except in accordance with the terms  of
// that agreement.
//
// Copyright 2023 Discovery Machine, Inc. All Rights Reserved.
//=====================================================================

namespace MUSICLibrary.Interfaces
{
    /// <summary>
    /// The purpose of this interface is to define
    /// the functions which create constructs.
    /// </summary>
    public interface IConstructCreator
    {
        IConstruct CreateLocalConstruct(IConstructCreateInfo createInfo);
    }
}
