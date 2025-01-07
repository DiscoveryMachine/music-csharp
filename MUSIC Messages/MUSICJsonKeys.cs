/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

namespace MUSICLibrary.MUSIC_Messages
{
    /// <summary>
    /// The purpose of this class is to define the JSON keys
    /// for every json MUSIC message.
    /// </summary>
    internal static class MUSICJsonKeys
    {
        internal const string HEADER = "header";
        internal const string ORIGIN_ID = "originID";
        internal const string RECEIVER_ID = "receiverID";
        internal const string REQUEST_ID = "requestID";

        internal const string FORCE = "force";
        internal const string ENTITY_TYPE = "entityType";
        internal const string LOCATION = "location";
        internal const string ORIENTATION = "orientation";
        internal const string VELOCITY = "velocity";
        internal const string DAMAGE_RECORD = "damageRecord";
        internal const string DEAD_RECK = "deadReck";

        internal const string CONSTRUCT_ID = "constructID";
        internal const string GHOSTED_ID = "ghostedID";
        internal const string PRIMARY_CONTROLLER_ID = "primaryControllerID";
        internal const string CURRENT_CONTROLLER_ID = "currentControllerID";
        internal const string CONSTRUCT_INFO_RECORD = "constructInformationRecord";
        internal const string CONSTRUCT_RENDER = "constructRender";
        internal const string CONSTRUCT_TYPE = "constructType";
        internal const string CALLSIGN = "callsign";
        internal const string CONSTRUCT_NAME = "constructName";
        internal const string INTERACTION_RECORD = "interactionRecord";

        internal const string PSI = "psi";
        internal const string THETA = "theta";
        internal const string PHI = "phi";

        internal const string INTERACTION_NAME = "interactionName";
        internal const string INTERACTION_TYPE = "interactionType";
        internal const string INTERACTION_DATA = "interactionData";

        internal const string GAINED_CONTROL_OF = "gainedControlOf";

        internal const string STATE_DATA = "stateData";

        internal const string PERCEPTION_RECORDS = "perceptionRecords";
        internal const string NUM_PERCEPTIONS = "numPerceptions";

        internal const string CURRENT_WAYPOINT_INDEX = "currentWaypointIndex";
        internal const string WAYPOINT_RECORDS = "waypointRecords";
        internal const string NUM_WAYPOINTS = "numWaypoints";

        internal const string EVENT_TYPE = "eventType";
        internal const string EVENT_DATA = "eventData";

        internal const string TARGET_CONSTRUCT = "targetConstruct";
        internal const string CONTEXT = "context";

        internal const string PROPOSED_CONTROLLER = "proposedController";

        internal const string CONSTRUCT_CALLSIGN = "constructCallsign";
        internal const string CONSTRUCT_LOCATION = "constructLocation";
        internal const string CONSTRUCT_ORIENTATION = "constructOrientation";
        internal const string COMMAND_IDENTIFIER = "commandIdentifier";

        internal const string ENVIRONMENT_NAME = "environmentName";
        internal const string ENVIRONMENT_META_DATA = "environmentMetadata";

        internal const string CONSTRUCT_PARAMETERS = "constructParameters";

        internal const string STATUS = "status";

        internal const string OPTIONAL_DATA = "optionalData";

        internal const string CONTROLLED_CONSTRUCT = "controlledConstruct";

        internal const string MESSAGES = "messages";
        internal const string TIMEOUT = "timeout";

        internal const string REMOVED_CONSTRUCT = "removedConstruct";

        internal const string NEW_TIME = "newTime";

        internal const string OLD_ID = "oldID";
        internal const string NEW_ID = "newID";

        internal const string SIM_TIME = "simTime";

        internal const string SITE_ID = "siteID";
        internal const string APP_ID = "appID";
        internal const string ENTITY_ID = "entityID";
    }
}
