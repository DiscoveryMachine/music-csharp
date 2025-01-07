/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

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
