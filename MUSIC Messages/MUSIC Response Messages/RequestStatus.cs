/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

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
