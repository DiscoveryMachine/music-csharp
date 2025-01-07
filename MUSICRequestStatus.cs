/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using MUSICLibrary.Interfaces;
using MUSICLibrary.MUSIC_Messages.MUSIC_Response_Messages;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace MUSICLibrary
{
    public class MUSICRequestStatus
    {
        public IConstruct Sender { get; set; }
        public IConstruct Receiver { get; set; }
        public uint ExerciseID { get; set; }

        /// <summary>
        /// A unique ID per construct's request monitor for each request that construct creates.
        /// </summary>
        public uint RequestID { get; private set; }

        private RequestStatus? LastStatus { get; set; }


        private Dictionary<RequestStatus?, Action<JObject>> handlers;
        private Dictionary<RequestStatus?, JObject> notifiedStatuses;

        public MUSICRequestStatus(uint exerciseID, uint requestID, IConstruct sender, IConstruct receiver)
        {
            handlers = new Dictionary<RequestStatus?, Action<JObject>>();
            notifiedStatuses = new Dictionary<RequestStatus?, JObject>();
            Sender = sender;
            Receiver = receiver;
            RequestID = requestID;
            ExerciseID = exerciseID;
            LastStatus = null;
        }

        public void Notify(RequestStatus status, JObject optionalData = null)
        {
            if (LastStatusWasTerminal())
                throw new InvalidOperationException("Notifications are no longer accepted by this request as it has already received a request terminating status.");

            if (handlers.ContainsKey(status))
                ExecuteHandlerThenRemoveFromStatusSet(status, optionalData);
            else
                notifiedStatuses.Add(status, optionalData);

            LastStatus = status;
        }

        public void RegisterHandler(RequestStatus status, Action<JObject> handler)
        {
            handlers.Add(status, handler);

            if (notifiedStatuses.ContainsKey(status))
                ExecuteHandlerThenRemoveFromStatusSet(status, notifiedStatuses[status]);
        }

        public bool LastStatusWasTerminal()
        {
            return LastStatus == RequestStatus.Aborted || LastStatus == RequestStatus.Complete || LastStatus == RequestStatus.PartiallyComplete;
        }

        private void ExecuteHandlerThenRemoveFromStatusSet(RequestStatus status, JObject optionalData = null)
        {
            handlers[status](optionalData);
            notifiedStatuses.Remove(status);
        }

    }
}
