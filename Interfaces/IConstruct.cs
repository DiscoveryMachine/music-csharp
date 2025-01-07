/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

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
