//DISCOVERY MACHINE, INC. PROPRIETARY INFORMATION
//
//This software is supplied under the terms of a license agreement
//or nondisclosure agreement with Discovery Machine, Inc. and may
//not be copied or disclosed except in accordance with the terms  of
//that agreement.
//
//Copyright 2022-23 Discovery Machine, Inc. All Rights Reserved.
//=====================================================================

using System;
using MUSICLibrary.Endpoints;
using MUSICLibrary.MUSIC_Messages;

namespace MUSICLibrary.Interfaces
{
    public interface IMUSICLibrary : IConstructCreator
    {
        void SubscribeToMUSIC();
        void UnsubscribeToMUSIC();
        bool IsSubscribed();
        MUSICEndpoint GetEndpoint(Type type);
        IMUSICLibrary InitializeEndpoint(Type type);
        IMUSICLibrary RemoveMUSICEndpoint(Type endpointType);
        void Transmit(MUSICMessage message);
    }
}
