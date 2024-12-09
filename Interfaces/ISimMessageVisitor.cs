//=====================================================================
//DISCOVERY MACHINE, INC. PROPRIETARY INFORMATION
//
//This software is supplied under the terms of a license agreement
//or nondisclosure agreement with Discovery Machine, Inc. and may
//not be copied or disclosed except in accordance with the terms  of
//that agreement.
//
//Copyright 2022 Discovery Machine, Inc. All Rights Reserved.
//=====================================================================

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
