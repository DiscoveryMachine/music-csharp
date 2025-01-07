/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Newtonsoft.Json.Linq;

namespace MUSICLibrary.Interfaces
{
    public interface IToJSON
    {
        JObject ToJsonObject();
    }
}
