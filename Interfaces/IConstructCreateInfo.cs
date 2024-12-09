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

using System;
using MUSICLibrary.MUSIC_Messages;

namespace MUSICLibrary.Interfaces
{
    public interface IConstructCreateInfo
    {
        string QualifiedName { get; set; }
        string Callsign { get; set; }

        /// <summary>
        /// [[Do not manually set this property]]
        /// This property is only able to be set correctly by the construct factory because
        /// the 'EntityID' property of the EntityIDRecord for a construct is unknown before its instantiation.
        /// </summary>
        EntityIDRecord ConstructID { get; set; }

        Type ConstructType { get; set; }
        IConstructRepository Repository { get; set; }
        IMUSICTransmitter Transmitter { get; set; }
        MUSICVector3 ConstructLocation { get; set; }
        MUSICVector3 ConstructOrientation { get; set; }
    }
}
