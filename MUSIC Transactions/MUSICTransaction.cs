using MUSICLibrary.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUSICLibrary.MUSIC_Transactions
{
    public class MUSICTransaction
    {
        public event Action<MUSICTransaction> TransactionExecuted;
        public JObject constructDataMessage;

        public IMUSICTransmitter transmitter;

        public MUSICTransaction(IMUSICTransmitter transmitter)
        {
            this.transmitter = transmitter;
        }

        public int GetOperationCount()
        {
            return TransactionExecuted.GetInvocationList().Length;
        }

        public virtual void OnTransactionExecuted()
        {
            TransactionExecuted?.Invoke(this);
        }
    }
}
