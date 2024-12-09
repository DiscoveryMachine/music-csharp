//=====================================================================
// DISCOVERY MACHINE, INC. PROPRIETARY INFORMATION
//
// This software is supplied under the terms of a license agreement
// or nondisclosure agreement with Discovery Machine, Inc. and may
// not be copied or disclosed except in accordance with the terms of
// that agreement.
//
// Copyright 2022 Discovery Machine, Inc. All Rights Reserved.
//=====================================================================

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
