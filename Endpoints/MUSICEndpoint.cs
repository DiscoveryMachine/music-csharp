/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using MUSICLibrary.Interfaces;
using MUSICLibrary.MUSIC_Messages;
using MUSICLibrary.MUSIC_Messages.Construct_Data;
using MUSICLibrary.MUSIC_Messages.MUSIC_Request_Messages;
using MUSICLibrary.MUSIC_Messages.MUSIC_Response_Messages;
using MUSICLibrary.MUSIC_Messages.Targeted_MUSIC_Messages;
using System;

namespace MUSICLibrary.Endpoints
{
    public abstract class MUSICEndpoint : IMUSICMessageVisitor
    {
        public SiteAndAppID SiteAndAppID { get; }
        public EntityIDRecord OriginID { get; }
        public uint ExerciseID { get; }

        public IMUSICMessageVisitor InternalVisitor { get; }
        public IMUSICMessageVisitor ExternalVisitor { get; }
        public IMessageFilter MessageFilter { get; }

        public bool Subscribed { get; protected set; }

        public string LastMessage { get; set; }

        public MUSICEndpoint(LibraryConfiguration config)
        {
            SiteAndAppID = config.SiteAndAppID;
            ExerciseID = config.ExerciseID;
            MessageFilter = config.MessageFilter;
            InternalVisitor = config.InternalMessageVisitor;
            ExternalVisitor = config.ExternalMessageVisitor;
            OriginID = new EntityIDRecord(SiteAndAppID, 0);
        }

        public abstract void SubscribeToMUSIC();

        public abstract void UnsubscribeFromMUSIC();

        public void Transmit(MUSICMessage message)
        {
            message.AcceptVisitor(this);
        }

        public void Receive(MUSICMessage message)
        {
            bool shouldDiscard = MessageFilter.ShouldDiscard(message);
            if (!shouldDiscard)
            {
                message.AcceptVisitor(InternalVisitor);

                if (ExternalVisitor != null)
                {
                    message.AcceptVisitor(ExternalVisitor);
                }
            }
            else

            MessageFilter.OnHandledMessage(message);
        }

        public abstract void VisitMUSICEvent(MUSICEventMessage message);
        public abstract void VisitStateField(StateFieldMessage message);
        public abstract void VisitConstructData(ConstructDataMessage message);
        public abstract void VisitWaypointData(WaypointDataMessage message);
        public abstract void VisitPerceptionData(PerceptionDataMessage message);
        public abstract void VisitControlGained(ControlGainedMessage message);

        //request MUSIC messages
        public abstract void VisitControlRequest(ControlRequestMessage message);
        public abstract void VisitControlTransferRequest(ControlTransferRequestMessage message);
        public abstract void VisitCreateConstructRequest(CreateConstructRequestMessage message);
        public abstract void VisitCreateEnvironmentRequest(CreateEnvironmentRequestMessage message);
        public abstract void VisitFinalizeScenarioRequest(FinalizeScenarioRequestMessage message);
        public abstract void VisitParameterizeConstructRequest(ParameterizeConstructRequestMessage message);
        public abstract void VisitPrimaryControlRequest(PrimaryControlRequestMessage message);
        public abstract void VisitInteractionRequest(InteractionRequestMessage message);

        //response MUSIC messages
        public abstract void VisitControlResponse(ControlResponseMessage message);
        public abstract void VisitControlTransferResponse(ControlTransferResponseMessage message);
        public abstract void VisitCreateConstructResponse(CreateConstructResponseMessage message);
        public abstract void VisitCreateEnvironmentResponse(CreateEnvironmentResponseMessage message);
        public abstract void VisitFinalizeScenarioResponse(FinalizeScenarioResponseMessage message);
        public abstract void VisitParameterizeConstructResponse(ParameterizeConstructResponseMessage message);
        public abstract void VisitPrimaryControlResponse(PrimaryControlResponseMessage message);
        public abstract void VisitInteractionResponse(InteractionResponseMessage message);

        //targeted MUSIC messages
        public abstract void VisitControlInitiated(ControlInitiatedMessage message);
        public abstract void VisitControlLost(ControlLostMessage message);
        public abstract void VisitControlReclamation(ControlReclamationMessage message);
        public abstract void VisitControlRegained(ControlRegainedMessage message);
        public abstract void VisitControlReleased(ControlReleasedMessage message);
        public abstract void VisitControlRelinquished(ControlRelinquishedMessage message);
        public abstract void VisitDisplayMessages(DisplayMessagesMessage message);
        public abstract void VisitPrimaryControlRelinquished(PrimaryControlRelinquishedMessage message);
        public abstract void VisitRemoveConstruct(RemoveConstructMessage message);
        public abstract void VisitRequestSimulationTime(RequestSimulationTimeMessage message);
        public abstract void VisitScenarioStart(ScenarioStartMessage message);
        public abstract void VisitSetCurrentController(SetCurrentControllerMessage message);
        public abstract void VisitSetSimulationTime(SetSimulationTimeMessage message);
        public abstract void VisitSimulationTime(SimulationTimeMessage message);
        public abstract void VisitStopConstruct(StopConstructMessage message);
        public abstract void VisitTransferConstructID(TransferConstructIDMessage message);
    }
}
