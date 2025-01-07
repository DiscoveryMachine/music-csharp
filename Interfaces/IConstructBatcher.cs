/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using MUSICLibrary.Visitors.Default_Internal_Message_Visitor.Construct_Repository.Batcher;

namespace MUSICLibrary.Interfaces
{
    public interface IConstructBatcher
    {
        int ActiveThreadCount { get; }
        void StartBatchThreads(IBatchOperation op, ConstructBatches batches);
        void StopBatchThreads();
    }
}
