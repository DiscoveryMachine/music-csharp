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
using MUSICLibrary.MUSIC_Messages.Construct_Data;
using MUSICLibrary.MUSIC_Messages.MUSIC_Request_Messages;
using MUSICLibrary.MUSIC_Messages.MUSIC_Response_Messages;
using MUSICLibrary.MUSIC_Messages.Targeted_MUSIC_Messages;

namespace MUSICLibrary.Interfaces
{
    public interface IConstructMessageVisitor
    {
        //MUSICMessage
        void VisitStateField(StateFieldMessage message);
        void VisitControlGained(ControlGainedMessage message);
        void VisitConstructData(ConstructDataMessage message);
        void VisitWaypointData(WaypointDataMessage message);
        void VisitPerceptionData(PerceptionDataMessage message);

        //MUSICRequestMessage
        void VisitControlRequest(ControlRequestMessage message);
        void VisitPrimaryControlRequest(PrimaryControlRequestMessage message);
        void VisitControlTransferRequest(ControlTransferRequestMessage message);
        void VisitInteractionRequest(InteractionRequestMessage message);
        
        //MUSICResponseMessage
        void VisitControlResponse(ControlResponseMessage message);
        void VisitPrimaryControlResponse(PrimaryControlResponseMessage message);
        void VisitControlTransferResponse(ControlTransferResponseMessage message);
        void VisitInteractionResponse(InteractionResponseMessage message);

        //TargetedMUSICMessage
        void VisitSetCurrentController(SetCurrentControllerMessage message);
        void VisitControlLost(ControlLostMessage message);
        void VisitControlRegained(ControlRegainedMessage message);
        void VisitControlRelinquished(ControlRelinquishedMessage message);
        void VisitPrimaryControlRelinquished(PrimaryControlRelinquishedMessage message);
        void VisitControlReclamation(ControlReclamationMessage message);
        void VisitSimulationTime(SimulationTimeMessage message);
    }
}
