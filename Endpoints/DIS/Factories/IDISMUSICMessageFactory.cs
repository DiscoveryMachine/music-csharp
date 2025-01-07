/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using MUSICLibrary.MUSIC_Messages;

namespace MUSICLibrary.Endpoints.DIS.Factories
{
    public interface IDISMUSICMessageFactory
    {
        MUSICMessage Create(byte[] pdu);
    }
}
