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

using Newtonsoft.Json.Linq;
using MUSICLibrary.MUSIC_Messages;
using MUSICLibrary.MUSIC_Messages.Construct_Data;

namespace MUSICLibrary.Interfaces
{
    public interface IConstruct : IMUSICMessageVisitor
    {
        EntityIDRecord GetID();
        bool IsRemote();
        ConstructDataMessage GetConstructData();
        StateFieldMessage GetStateFieldData();
        bool Parameterize(JObject json);
        MUSICRequestStatus RequestPrimaryControl(IConstruct sender);
        void RelinquishPrimaryControl(IConstruct sender);
        void RelinquishControl(IConstruct sender);
        void SetCurrentController(IConstruct controller);
        void OnPrimaryControlRelinquished(IConstruct construct);
        void OnControlRelinquished(IConstruct construct);
        void OnControlLost(IConstruct construct);
        void OnControlRegained(IConstruct construct);
        void OnControlRelamation(IConstruct construct);
        void OnControlGained(IConstruct construct);
        void OnStopConstruct();
        MUSICRequestMonitor GetRequestMonitor();

    }
}
