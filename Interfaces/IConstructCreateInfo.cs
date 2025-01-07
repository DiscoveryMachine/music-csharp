/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

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
