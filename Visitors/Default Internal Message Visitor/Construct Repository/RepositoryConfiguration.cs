/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using MUSICLibrary.Interfaces;

namespace MUSICLibrary.Visitors.Default_Internal_Message_Visitor.Construct_Repository
{
    public class RepositoryConfiguration
    {
        public IConstructBatcher LocalBatcher { get; set; }
        public IConstructBatcher RemoteBatcher { get; set; }
        public uint LocalBatchCount { get; set; }
        public uint RemoteBatchCount { get; set; }
        public uint LocalBatchInterval { get; set; }
        public uint RemoteBatchInterval { get; set; }
    }
}
