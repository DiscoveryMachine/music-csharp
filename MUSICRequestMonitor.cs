/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using MUSICLibrary.Interfaces;
using MUSICLibrary.MUSIC_Messages.MUSIC_Response_Messages;

namespace MUSICLibrary
{
    public class MUSICRequestMonitor
    {
        private HashSet<MUSICRequestStatus> AliveRequests { get; set; }
        private readonly object hashSetLock = new object();

        private uint nextRequestID;
        private uint exerciseID;

        public MUSICRequestMonitor(uint exerciseID)
        {
            this.exerciseID = exerciseID;
            nextRequestID = 0;
            AliveRequests = new HashSet<MUSICRequestStatus>();
        }

        /// <summary>
        /// Creates then adds a new request to AliveRequests by instantiating its own MUSICRequestID.
        /// </summary>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public MUSICRequestStatus CreateRequest(IConstruct sender, IConstruct receiver)
        {
            MUSICRequestStatus status = new MUSICRequestStatus(exerciseID, nextRequestID++, sender, receiver);
            lock (hashSetLock)
            {
                AliveRequests.Add(status);
            }
            return status;
        }

        /// <summary>
        /// Creates then adds a request to AliveRequests using the given MUSICRequestID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public MUSICRequestStatus CreateRequest(uint id, IConstruct sender, IConstruct receiver)
        {
            MUSICRequestStatus status = new MUSICRequestStatus(exerciseID, id, sender, receiver);
            lock (hashSetLock)
            {
                AliveRequests.Add(status);
            }
            return status;
        }

        public MUSICRequestStatus GetRequestByID(uint requestID)
        {
            try
            {
                return AliveRequests
                    .Where((status) => { return status.RequestID == requestID; })
                    .First();
            }
            catch (InvalidOperationException e)
            {
                Console.Error.WriteLine(e.StackTrace);
                Console.Error.WriteLine(e.Message);
                throw new RequestStatusNotFoundException();
            }
        }

        public void Reset()
        {
            lock (hashSetLock)
            {
                AliveRequests
                .ToList()
                .ForEach(status => status.Notify(RequestStatus.Aborted));
                AliveRequests.Clear();
            }
        }

        public void RemoveRequestStatus(MUSICRequestStatus status)
        {
            lock (hashSetLock)
            {
                AliveRequests.Remove(status);
            }
        }

        public int GetAliveRequestsCount()
        {
            return AliveRequests.Count;
        }

        public bool ContainsRequest(uint requestID)
        {
            try
            {
                var status = GetRequestByID(requestID);
                return status != null;
            }
            catch (RequestStatusNotFoundException)
            {
                return false;
            }
        }

        public class RequestStatusNotFoundException : Exception
        {
            public RequestStatusNotFoundException()
            {
            }

            public RequestStatusNotFoundException(string message) : base(message)
            {
            }
        }
    }
}
