//=====================================================================
// DISCOVERY MACHINE, INC. PROPRIETARY INFORMATION
//
// This software is supplied under the terms of a license agreement
// or nondisclosure agreement with Discovery Machine, Inc. and may
// not be copied or disclosed except in accordance with the terms of
// that agreement.
//
// Copyright 2022-23 Discovery Machine, Inc. All Rights Reserved.
//=====================================================================

using MUSICLibrary.Interfaces;
using MUSICLibrary.MUSIC_Messages;
using MUSICLibrary.MUSIC_Messages.Targeted_MUSIC_Messages;

namespace MUSICLibrary.MessageFilters
{
    public class DefaultMessageFilter : IMessageFilter
    {
        private LibraryConfiguration config;

        public DefaultMessageFilter(LibraryConfiguration config)
        {
            this.config = config;
        }

        public void OnHandledMessage(MUSICMessage message)
        {

        }

        public virtual bool ShouldDiscard(MUSICMessage message)
        {
            if (IsLoopbackTraffic(message) || IsForDifferentExercise(message) || !IsIntendedReceiver(message))
                return true;

            return false;
        }

        private bool IsForDifferentExercise(MUSICMessage message)
        {
            return message.MUSICHeader.ExerciseID != config.ExerciseID;
        }

        private bool IsLoopbackTraffic(MUSICMessage message)
        {
            return
                message.OriginID.SiteID == config.SiteAndAppID.SiteID &&
                message.OriginID.AppID == config.SiteAndAppID.AppID;
        }

        private bool IsIntendedReceiver(MUSICMessage message)
        {
            TargetedMUSICMessage targetedMessage = message as TargetedMUSICMessage;
            if (targetedMessage == null)
                return true;

            return targetedMessage.ReceiverID.SiteID == config.SiteAndAppID.SiteID &&
                targetedMessage.ReceiverID.AppID == config.SiteAndAppID.AppID;
        }
    }
}
