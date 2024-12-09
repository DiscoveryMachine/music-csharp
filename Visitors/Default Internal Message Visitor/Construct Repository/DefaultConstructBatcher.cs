//=====================================================================
// DISCOVERY MACHINE, INC. PROPRIETARY INFORMATION
//
// This software is supplied under the terms of a license agreement
// or nondisclosure agreement with Discovery Machine, Inc. and may
// not be copied or disclosed except in accordance with the terms of
// that agreement.
//
// Copyright 2022-23 Discovery Machine, Inc. All Rights Reserved.
//=====================================================================

using MUSICLibrary.Interfaces;
using MUSICLibrary.Visitors.Default_Internal_Message_Visitor.Construct_Repository.Batcher;
using System;
using System.Collections.Generic;
using System.Timers;

namespace MUSICLibrary.Visitors.Default_Internal_Message_Visitor.Construct_Repository
{
    public class DefaultConstructBatcher : IConstructBatcher
    {
        protected List<Timer> timers;

        private readonly uint intervalMilliseconds;
        private IMUSICLibrary lib;

        public int ActiveThreadCount
        {
            get
            {
                int numActive = 0;
                foreach (var timer in timers)
                    if (timer.Enabled)
                        numActive++;
                return numActive;
            }
        }

        public DefaultConstructBatcher(IMUSICLibrary lib, uint intervalSeconds)
        {
            intervalMilliseconds = intervalSeconds * 1000;
            timers = new List<Timer>();
            this.lib = lib;
        }

        public void StartBatchThreads(IBatchOperation op, ConstructBatches batches)
        {
            foreach(var batch in batches.Constructs)
            {
                Timer timer = new Timer(intervalMilliseconds);
                timer.Elapsed += (object sender, ElapsedEventArgs args) => 
                {
                    try
                    {
                        if (!lib.IsSubscribed())
                            StopBatchThreads();

                        op.ApplyTo(batch);
                    }
                    catch (Exception)
                    {
                        StopBatchThreads();
                    }
                };
                timers.Add(timer);
                timer.Enabled = true;
            }
        }

        public void StopBatchThreads()
        {
            foreach (var timer in timers)
            {
                timer.Enabled = false;
                timer.Dispose();
            }
            timers.Clear();
        }
    }
}
