/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using MUSICLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;

namespace MUSICLibrary.MUSIC_Transactions
{
    public class MUSICTransactionMonitor
    {
        public IMUSICTransmitter transmitter;

        public Dictionary<int, MUSICTransaction> threadIDToTransactionMap;

        public MUSICTransactionMonitor(IMUSICTransmitter transmitter)
        { 
            this.transmitter = transmitter;
            threadIDToTransactionMap = new Dictionary<int, MUSICTransaction>();
        }

        public void BeginTransaction()
        {
            int currentThreadID = Thread.CurrentThread.ManagedThreadId;
            if (threadIDToTransactionMap.ContainsKey(currentThreadID))
            {
                throw new TransactionAlreadyExistsForThreadException("A transaction already exists for the current thread"); 
            }

            MUSICTransaction transaction = new MUSICTransaction(transmitter);
            threadIDToTransactionMap.Add(currentThreadID, transaction);
        }

        public bool InTransaction()
        {
            return threadIDToTransactionMap.Count > 0; 
        }
    }

    public class TransactionAlreadyExistsForThreadException : Exception
    {
        public TransactionAlreadyExistsForThreadException(String message) : base(message)
        {

        }
    }
}
