/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System;
using System.Collections.Generic;
using MUSICLibrary.Interfaces;
using MUSICLibrary.MUSIC_Messages;
using MUSICLibrary.MUSIC_Messages.Construct_Data;
using MUSICLibrary.MUSIC_Messages.MUSIC_Request_Messages;
using MUSICLibrary.MUSIC_Messages.MUSIC_Response_Messages;
using MUSICLibrary.MUSIC_Messages.Targeted_MUSIC_Messages;
using MUSICLibrary.Visitors.Default_Internal_Message_Visitor.Construct_Factory;
using Newtonsoft.Json.Linq;

namespace MUSICLibrary.Visitors
{
    public class DefaultInternalMessageVisitor : IInternalMessageVisitor
    {
        public IConstructFactory ConstructFactory { get; private set; }
        public IConstructRepository ConstructRepository { get; private set; }
        public IMUSICTransmitter Transmitter { get; private set; }
        private Dictionary<string, List<Action<JObject>>> EventHandlers { get; set; }

        private LibraryConfiguration config;

        #region Error messages

        private const string LOCAL_EVENT_MESSAGE_ERROR_MSG =
            "[Warning]: The OriginID's SiteAndAppID of the MUSICEventMessage matches that of the local simulation. " +
            "This simulation should not receive a message to handle an event that a local construct or the simulation itself has triggered. " +
            "A common cause of this error is an errorneous OriginID.";

        private const string WRONG_EXERCISE_ID_ERROR_MSG_FORMAT =
             "[Warning]: The event message's ExerciseID did not match the stored ExerciseID. " +
             "DefaultInternalMessageVisitorExerciseID: {{{0}}}, MUSICEventMessage ExerciseID: {{{1}}}\n" +
             "1 of 2 things may cause this exception to be thrown:\n\t" +
             "1: The message has an erroneous ExerciseID that somehow made it through message filtering.\n\t" +
             "2: This instance of a DefaultInternalMessageVisitor was instantiated with an ExerciseID that does not match the current simulation.";

        private const string EVENT_NOT_FOUND_ERROR_MSG_FORMAT =
            "[Warning]: The event of type: {{{0}}} could not be found within the EventHandlers dictionary. " +
            "A common cause of this error is not registering the appropriate MUSICEventType before the library subscribes to MUSIC.";

        private const string HANDLER_NOT_FOUND_ERROR_MSG_FORMAT =
            "[Warning]: An event handler was not found for event type: {{{0}}}. " +
            "When registering a handler for this event, ensure that the handler function registered to the event " +
            "type is not null.";

        private const string CONSTRUCT_NOT_FOUND_ERROR_MSG =
            "[Warning]: The default internal message visitor could not make the " +
            "state field message accept the construct visitor. Construct not found.";

        #endregion 

        public DefaultInternalMessageVisitor(IMUSICTransmitter lib, LibraryConfiguration config)
        {
            Transmitter = lib;
            this.config = config;
        }

        /// <summary>
        /// Initializes a local construct by creating a local construct, adding it to the repository, 
        /// then transmitting the construct data of the newly created construct.
        /// </summary>
        /// <param name="transmitter"></param>
        /// <param name="constructType"></param>
        /// <returns></returns>
        public IConstruct CreateLocalConstruct(IConstructCreateInfo createInfo)
        {
            ConstructFactory.RegisterLocalConstruct(createInfo);
            IConstruct localConstruct = ConstructFactory.Create(createInfo);
            ConstructRepository.AddConstruct(localConstruct);
            createInfo.Transmitter.Transmit(localConstruct.GetConstructData());
            return localConstruct;
        }

        public void RegisterEventHandler(string eventName, Action<JObject> eventHandler)
        {
            if (!EventHandlers.ContainsKey(eventName))
            {
                EventHandlers[eventName] = new List<Action<JObject>>();
            }
            EventHandlers[eventName].Add(eventHandler);
        }

        public void DeregisterEventHandler(string eventName)
        {
            EventHandlers.Remove(eventName);
        }

        public void Initialize(IConstructFactory constructFactory, IConstructRepository constructRepository)
        {
            ConstructFactory = constructFactory;
            ConstructRepository = constructRepository;
            EventHandlers = new Dictionary<string, List<Action<JObject>>>();
        }

        public void VisitConstructData(ConstructDataMessage message)
        {
            if (ConstructRepository.ConstructExists(message.OriginID))
            {
                IConstruct construct = ConstructRepository.GetConstructByID(message.OriginID);

                if (!construct.IsRemote())
                    throw new InvalidOperationException("Local constructs should never receive an update through a construct data message.");

                construct.GetConstructData().Update(message);
            }
            else
                ConstructRepository.AddConstruct(ConstructFactory.Create(message));
        }

        public void VisitInteractionRequest(InteractionRequestMessage message)
        {
            message.AcceptVisitor(ConstructRepository.GetConstructByID(message.ReceiverID));
        }

        public void VisitInteractionResponse(InteractionResponseMessage message)
        {
            message.AcceptVisitor(ConstructRepository.GetConstructByID(message.ReceiverID));
        }

        public void VisitMUSICEvent(MUSICEventMessage message)
        {
            if (message.MUSICHeader.ExerciseID != config.ExerciseID)
                throw new InvalidOperationException(
                    string.Format(WRONG_EXERCISE_ID_ERROR_MSG_FORMAT, config.ExerciseID, message.MUSICHeader.ExerciseID));

            if (message.OriginID.GetSiteAndApp().Equals(config.SiteAndAppID))
                throw new InvalidOperationException(LOCAL_EVENT_MESSAGE_ERROR_MSG);

            if (!EventHandlers.ContainsKey(message.EventType))
                throw new KeyNotFoundException(string.Format(EVENT_NOT_FOUND_ERROR_MSG_FORMAT, message.EventType));

            TriggerEvent(message.EventType, message.EventData);
        }

        public void TriggerEvent(string eventType, JObject eventData)
        {
            foreach (var handler in EventHandlers[eventType])
            {
                if (handler is null)
                    throw new ArgumentNullException(string.Format(HANDLER_NOT_FOUND_ERROR_MSG_FORMAT, eventType));

                handler.Invoke(eventData);
            }
        }

        public void VisitStateField(StateFieldMessage message)
        {
            if (ConstructRepository.ConstructExists(message.OriginID))
            {
                message.AcceptVisitor(ConstructRepository.GetConstructByID(message.OriginID));
            }
            else
            {
                throw new KeyNotFoundException(CONSTRUCT_NOT_FOUND_ERROR_MSG);
            }
        }

        public void VisitControlGained(ControlGainedMessage message)
        {
            if (ConstructRepository.ConstructExists(message.ReceiverID))
            {
                message.AcceptVisitor(ConstructRepository.GetConstructByID(message.ReceiverID));
            }
            else
            {
                throw new KeyNotFoundException(CONSTRUCT_NOT_FOUND_ERROR_MSG);
            }
        }

        public void VisitWaypointData(WaypointDataMessage message)
        {
            if (ConstructRepository.ConstructExists(message.OriginID))
            {
                message.AcceptVisitor(ConstructRepository.GetConstructByID(message.OriginID));
            }
            else
            {
                throw new KeyNotFoundException(CONSTRUCT_NOT_FOUND_ERROR_MSG);
            }
        }

        public void VisitPerceptionData(PerceptionDataMessage message)
        {
            if (ConstructRepository.ConstructExists(message.OriginID))
            {
                message.AcceptVisitor(ConstructRepository.GetConstructByID(message.OriginID));
            }
            else
            {
                throw new KeyNotFoundException(CONSTRUCT_NOT_FOUND_ERROR_MSG);
            }
        }

        public void VisitControlRequest(ControlRequestMessage message)
        {
            if (ConstructRepository.ConstructExists(message.ReceiverID))
            {
                message.AcceptVisitor(ConstructRepository.GetConstructByID(message.ReceiverID));
            }
            else
            {
                throw new KeyNotFoundException(CONSTRUCT_NOT_FOUND_ERROR_MSG);
            }
        }

        public void VisitPrimaryControlRequest(PrimaryControlRequestMessage message)
        {
            if (ConstructRepository.ConstructExists(message.ReceiverID))
            {
                message.AcceptVisitor(ConstructRepository.GetConstructByID(message.ReceiverID));
            }
            else
            {
                throw new KeyNotFoundException(CONSTRUCT_NOT_FOUND_ERROR_MSG);
            }
        }

        public void VisitControlTransferRequest(ControlTransferRequestMessage message)
        {
            if (ConstructRepository.ConstructExists(message.ReceiverID))
            {
                message.AcceptVisitor(ConstructRepository.GetConstructByID(message.ReceiverID));
            }
            else
            {
                throw new KeyNotFoundException(CONSTRUCT_NOT_FOUND_ERROR_MSG);
            }
        }

        public void VisitControlResponse(ControlResponseMessage message)
        {
            if (ConstructRepository.ConstructExists(message.ReceiverID))
            {
                message.AcceptVisitor(ConstructRepository.GetConstructByID(message.ReceiverID));
            }
            else
            {
                throw new KeyNotFoundException(CONSTRUCT_NOT_FOUND_ERROR_MSG);
            }
        }

        public void VisitPrimaryControlResponse(PrimaryControlResponseMessage message)
        {
            if (ConstructRepository.ConstructExists(message.ReceiverID))
            {
                message.AcceptVisitor(ConstructRepository.GetConstructByID(message.ReceiverID));
            }
            else
            {
                throw new KeyNotFoundException(CONSTRUCT_NOT_FOUND_ERROR_MSG);
            }
        }

        public void VisitControlTransferResponse(ControlTransferResponseMessage message)
        {
            if (ConstructRepository.ConstructExists(message.ReceiverID))
            {
                message.AcceptVisitor(ConstructRepository.GetConstructByID(message.ReceiverID));
            }
            else
            {
                throw new KeyNotFoundException(CONSTRUCT_NOT_FOUND_ERROR_MSG);
            }
        }

        public void VisitSetCurrentController(SetCurrentControllerMessage message)
        {
            if (ConstructRepository.ConstructExists(message.ReceiverID))
            {
                message.AcceptVisitor(ConstructRepository.GetConstructByID(message.ReceiverID));
            }
            else
            {
                throw new KeyNotFoundException(CONSTRUCT_NOT_FOUND_ERROR_MSG);
            }
        }

        public void VisitControlLost(ControlLostMessage message)
        {
            if (ConstructRepository.ConstructExists(message.ReceiverID))
            {
                message.AcceptVisitor(ConstructRepository.GetConstructByID(message.ReceiverID));
            }
            else
            {
                throw new KeyNotFoundException(CONSTRUCT_NOT_FOUND_ERROR_MSG);
            }
        }

        public void VisitControlRegained(ControlRegainedMessage message)
        {
            if (ConstructRepository.ConstructExists(message.ReceiverID))
            {
                message.AcceptVisitor(ConstructRepository.GetConstructByID(message.ReceiverID));
            }
            else
            {
                throw new KeyNotFoundException(CONSTRUCT_NOT_FOUND_ERROR_MSG);
            }
        }

        public void VisitControlRelinquished(ControlRelinquishedMessage message)
        {
            if (ConstructRepository.ConstructExists(message.ReceiverID))
            {
                message.AcceptVisitor(ConstructRepository.GetConstructByID(message.ReceiverID));
            }
            else
            {
                throw new KeyNotFoundException(CONSTRUCT_NOT_FOUND_ERROR_MSG);
            }
        }

        public void VisitPrimaryControlRelinquished(PrimaryControlRelinquishedMessage message)
        {
            if (ConstructRepository.ConstructExists(message.ReceiverID))
            {
                message.AcceptVisitor(ConstructRepository.GetConstructByID(message.ReceiverID));
            }
            else
            {
                throw new KeyNotFoundException(CONSTRUCT_NOT_FOUND_ERROR_MSG);
            }
        }

        public void VisitControlReclamation(ControlReclamationMessage message)
        {
            if (ConstructRepository.ConstructExists(message.ReceiverID))
            {
                message.AcceptVisitor(ConstructRepository.GetConstructByID(message.ReceiverID));
            }
            else
            {
                throw new KeyNotFoundException(CONSTRUCT_NOT_FOUND_ERROR_MSG);
            }
        }

        public void VisitSimulationTime(SimulationTimeMessage message)
        {
            //  no-op, there's nothing we can handle here, up to user to handle in their own message visitor
        }

        public void VisitControlInitiated(ControlInitiatedMessage message)
        {
            //  no-op, there's nothing we can handle here, up to user to handle in their own message visitor
        }

        public void VisitControlReleased(ControlReleasedMessage message)
        {
            if (ConstructRepository.ConstructExists(message.ReceiverID))
            {
                message.AcceptVisitor(ConstructRepository.GetConstructByID(message.ReceiverID));
            }
            else
            {
                throw new KeyNotFoundException(CONSTRUCT_NOT_FOUND_ERROR_MSG);
            }
        }

        public void VisitTransferConstructID(TransferConstructIDMessage message)
        {
            if (ConstructRepository.ConstructExists(message.ReceiverID))
            {
                message.AcceptVisitor(ConstructRepository.GetConstructByID(message.ReceiverID));
            }
            else
            {
                throw new KeyNotFoundException(CONSTRUCT_NOT_FOUND_ERROR_MSG);
            }
        }

        public void VisitDisplayMessages(DisplayMessagesMessage message)
        {
            //  no-op, there's nothing we can handle here, up to user to handle in their own message visitor
        }

        public void VisitStopConstruct(StopConstructMessage message)
        {
            if (ConstructRepository.ConstructExists(message.ReceiverID))
            {
                message.AcceptVisitor(ConstructRepository.GetConstructByID(message.ReceiverID));
            }
            else
            {
                throw new KeyNotFoundException(CONSTRUCT_NOT_FOUND_ERROR_MSG);
            }
        }

        public void VisitRemoveConstruct(RemoveConstructMessage message)
        {
            // no-op. there noting we can handle here, up to user to handle in their own message visitor
        }

        public void VisitSetSimulationTime(SetSimulationTimeMessage message)
        {
            //  no-op, there's nothing we can handle here, up to user to handle in their own message visitor
        }

        public void VisitRequestSimulationTime(RequestSimulationTimeMessage message)
        {
            //  no-op, there's nothing we can handle here, up to user to handle in their own message visitor
        }

        public void VisitCreateEnvironmentRequest(CreateEnvironmentRequestMessage message)
        {
            //  no-op, there's nothing we can handle here, up to user to handle in their own message visitor
        }

        public void VisitCreateEnvironmentResponse(CreateEnvironmentResponseMessage message)
        {
            //  no-op, there's nothing we can handle here, up to user to handle in their own message visitor
        }

        public void VisitFinalizeScenarioRequest(FinalizeScenarioRequestMessage message)
        {
            //  no-op, there's nothing we can handle here, up to user to handle in their own message visitor
        }

        public void VisitFinalizeScenarioResponse(FinalizeScenarioResponseMessage message)
        {
            //  no-op, there's nothing we can handle here, up to user to handle in their own message visitor
        }

        //TODO create proper unit tests since this has changed to actually perform the factory create here
        public void VisitCreateConstructRequest(CreateConstructRequestMessage message)
        {
            //noop
        }


        public void VisitCreateConstructResponse(CreateConstructResponseMessage message)
        {
            if (ConstructRepository.ConstructExists(message.ReceiverID))
            {
                message.AcceptVisitor(ConstructRepository.GetConstructByID(message.ReceiverID));
            }
            else
            {
                throw new KeyNotFoundException(CONSTRUCT_NOT_FOUND_ERROR_MSG);
            }
        }

        public void VisitParameterizeConstructRequest(ParameterizeConstructRequestMessage message)
        {
            //no op
        }

        public void VisitParameterizeConstructResponse(ParameterizeConstructResponseMessage message)
        {
            if (ConstructRepository.ConstructExists(message.ReceiverID))
            {
                message.AcceptVisitor(ConstructRepository.GetConstructByID(message.ReceiverID));
            }
            else
            {
                throw new KeyNotFoundException(CONSTRUCT_NOT_FOUND_ERROR_MSG);
            }
        }

        public void VisitScenarioStart(ScenarioStartMessage message)
        {
            //  no-op, there's nothing we can handle here, up to user to handle in their own message visitor
        }
    }

    public class EventNotFoundException : Exception
    {
        public EventNotFoundException() { }
        public EventNotFoundException(string message) : base(message) { }
    }

    public class HandlerNotFoundException : Exception
    {
        public HandlerNotFoundException() { }
        public HandlerNotFoundException(string message) : base(message) { }
    }
}
