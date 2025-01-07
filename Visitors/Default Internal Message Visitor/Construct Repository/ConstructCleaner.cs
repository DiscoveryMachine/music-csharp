/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System.Collections.Generic;
using MUSICLibrary.Interfaces;
using MUSICLibrary.MUSIC_Messages;
using System.Linq;
using MUSICLibrary.MUSIC_Messages.Construct_Data;

namespace MUSICLibrary.Visitors.Default_Internal_Message_Visitor.Construct_Repository
{
    public class ConstructCleaner : IBatchOperation
    {
        public void ApplyTo(Dictionary<EntityIDRecord, IConstruct> batch)
        {
            lock (batch)
            {
                var batchCopy = new List<KeyValuePair<EntityIDRecord, IConstruct>>(batch.ToList());
                foreach (var entry in batchCopy)
                    if (ShouldRemoveConstruct(entry.Value.GetConstructData()))
                        batch.Remove(entry.Key);
            }
        }

        private bool ShouldRemoveConstruct(ConstructDataMessage constructData)
        {
            bool isGhostedConstruct =
                constructData.ConstructRender == ConstructRender.GhostedConstruct ||
                constructData.ConstructRender == ConstructRender.GhostedLegacy;

            return
                isGhostedConstruct && constructData.GhostedID == null;
        }
    }
}
