/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

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
