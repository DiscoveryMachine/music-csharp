/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

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
