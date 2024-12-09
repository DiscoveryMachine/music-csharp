//=====================================================================
// DISCOVERY MACHINE, INC. PROPRIETARY INFORMATION
//
// This software is supplied under the terms of a license agreement
// or nondisclosure agreement with Discovery Machine, Inc. and may
// not be copied or disclosed except in accordance with the terms of
// that agreement.
//
// Copyright 2023 Discovery Machine, Inc. All Rights Reserved.
//=====================================================================

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
