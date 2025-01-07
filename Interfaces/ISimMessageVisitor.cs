/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using MUSICLibrary.MUSIC_Messages;
using MUSICLibrary.MUSIC_Messages.MUSIC_Request_Messages;
using MUSICLibrary.MUSIC_Messages.MUSIC_Response_Messages;
using MUSICLibrary.MUSIC_Messages.Targeted_MUSIC_Messages;

namespace MUSICLibrary.Interfaces
{
    public interface ISimMessageVisitor
    {
        void VisitMUSICEvent(MUSICEventMessage message);
        void VisitControlInitiated(ControlInitiatedMessage message);
        void VisitControlReleased(ControlReleasedMessage message);
        void VisitTransferConstructID(TransferConstructIDMessage message);
        void VisitDisplayMessages(DisplayMessagesMessage message);
        void VisitStopConstruct(StopConstructMessage message);
        void VisitRemoveConstruct(RemoveConstructMessage message);
        void VisitSetSimulationTime(SetSimulationTimeMessage message);
        void VisitRequestSimulationTime(RequestSimulationTimeMessage message);
        void VisitCreateEnvironmentRequest(CreateEnvironmentRequestMessage message);
        void VisitCreateEnvironmentResponse(CreateEnvironmentResponseMessage message);
        void VisitFinalizeScenarioRequest(FinalizeScenarioRequestMessage message);
        void VisitFinalizeScenarioResponse(FinalizeScenarioResponseMessage message);
        void VisitCreateConstructRequest(CreateConstructRequestMessage message);
        void VisitCreateConstructResponse(CreateConstructResponseMessage message);
        void VisitParameterizeConstructRequest(ParameterizeConstructRequestMessage message);
        void VisitParameterizeConstructResponse(ParameterizeConstructResponseMessage message);
        void VisitScenarioStart(ScenarioStartMessage message);
    }
}
