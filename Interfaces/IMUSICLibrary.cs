/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System;
using MUSICLibrary.Endpoints;
using MUSICLibrary.MUSIC_Messages;

namespace MUSICLibrary.Interfaces
{
    public interface IMUSICLibrary : IConstructCreator
    {
        void SubscribeToMUSIC();
        void UnsubscribeToMUSIC();
        bool IsSubscribed();
        MUSICEndpoint GetEndpoint(Type type);
        IMUSICLibrary InitializeEndpoint(Type type);
        IMUSICLibrary RemoveMUSICEndpoint(Type endpointType);
        void Transmit(MUSICMessage message);
    }
}
