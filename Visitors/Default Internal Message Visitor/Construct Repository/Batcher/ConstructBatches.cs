/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using MUSICLibrary.Interfaces;
using MUSICLibrary.MUSIC_Messages;
using System.Collections.Generic;
using System.Collections;
using System;
using static MUSICLibrary.Visitors.Default_Internal_Message_Visitor.Construct_Repository.ConstructRepository;

namespace MUSICLibrary.Visitors.Default_Internal_Message_Visitor.Construct_Repository.Batcher
{
    public class ConstructBatches : IEnumerable<IConstruct>
    {
        public List<Dictionary<EntityIDRecord, IConstruct>> Constructs { get; }
        public uint Count { get; set; }

        public ConstructBatches(uint batchCount)
        {
            Constructs = new List<Dictionary<EntityIDRecord, IConstruct>>();
            Count = 0;

            if (batchCount == 0)
            {
                throw new InvalidOperationException();
            }

            for(int i = 0; i < batchCount; i++)
            {
                Constructs.Add(new Dictionary<EntityIDRecord, IConstruct>());
            }
        }

        public void Add(IConstruct msg)
        {
            lock (Constructs)
            {
                try
                {
                    lock (Constructs[0])
                    {
                        // Find the smallest dictionary (batch) in constructs
                        Dictionary<EntityIDRecord, IConstruct> dict = Constructs[0];
                        for (int i = 0; i < Constructs.Count; i++)
                        {
                            if (Constructs[i].ContainsKey(msg.GetID()))
                            {
                                return;
                            }

                            if (Constructs[i].Count < dict.Count)
                            {
                                dict = Constructs[i];
                            }
                        }

                        // Add this new construct to dict and increment count
                        dict.Add(msg.GetID(), msg);
                        Count++;
                    }
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public void Remove(EntityIDRecord id)
        {
            lock (Constructs)
            {
                foreach (Dictionary<EntityIDRecord, IConstruct> dict in Constructs)
                {
                    if (dict.Remove(id))
                    {
                        Count--;
                        return;
                    }
                }
            }
        }

        public IConstruct GetConstructByID(EntityIDRecord id)
        {
            foreach (Dictionary<EntityIDRecord, IConstruct> dict in Constructs)
            {
                if (dict.ContainsKey(id))
                {
                    return dict[id];
                }
            }
            throw new ConstructNotFoundException();
        }

        public IEnumerator<IConstruct> GetEnumerator()
        {
            foreach (var batch in Constructs)
            {
                Dictionary<EntityIDRecord, IConstruct> batchCopy;
                lock (batch)
                    batchCopy = new Dictionary<EntityIDRecord, IConstruct>(batch);

                foreach (IConstruct value in batchCopy.Values)
                {
                    yield return value;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}
