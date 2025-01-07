/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

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
