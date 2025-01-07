/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System;
using System.Collections.Generic;
using MUSICLibrary.MUSIC_Messages;
using MUSICLibrary.MUSIC_Messages.Construct_Data;
using MUSICLibrary.MUSIC_Messages.MUSIC_Request_Messages;
using MUSICLibrary.MUSIC_Messages.MUSIC_Response_Messages;
using MUSICLibrary.MUSIC_Messages.Targeted_MUSIC_Messages;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace MUSICLibrary.Endpoints.Redis
{

    /// <summary>
    /// The Redis endpoint currently has no unit tests associated
    /// with it because every unit test for it would only end up testing
    /// the StackExchange Redis library which should not be done.
    /// Any problems that occur in production when using this
    /// endpoint type is most likely within this class.
    /// </summary>
    public class RedisEndpoint : MUSICEndpoint
    {
        /// <summary> The multiplexer managing connections to redis servers. </summary>
        public ConnectionMultiplexer Multiplexer { get; private set; }

        /// <summary> The subscriber object used to publish/subscribe to redis channels. </summary>
        public ISubscriber Subscriber { get; private set; }

        private Dictionary<string, string> PublishChannels { get; }
        private Dictionary<string, string> SubscribeChannels { get; }

        private Dictionary<uint, Action<JObject>> commandActions;
        private delegate MUSICMessage MessageInstantiationDel(JObject jsonObj);
        private string subscribedChannelID;
        private string endpointChannelID;

        public static string ConfigFilePath { get; set; }

        

        public RedisEndpoint(RedisLibraryConfiguration config) : base(config)
        {
                Multiplexer = ConnectionMultiplexer.Connect(string.Format("{0}:{1},password={2}",
                        config.RedisHost.Address.ToString(),
                        config.RedisHost.Port.ToString(),
                        config.Password
                        ));

                Subscriber = Multiplexer.GetSubscriber();

            subscribedChannelID = "Ex:" + config.ExerciseID + ":Site:*:App:*";
            endpointChannelID = "Ex:" + config.ExerciseID + ":Site:" + config.SiteAndAppID.SiteID + ":App:" + config.SiteAndAppID.AppID;

            SubscribeChannels = new Dictionary<string, string>()
            {
                { "ConstructData",                  "ConstructData:" + subscribedChannelID },
                { "StateFieldData",                 "StateFieldData:" + subscribedChannelID },
                { "PerceptionData",                 "PerceptionData:" + subscribedChannelID },
                { "WaypointData",                   "WaypointData:" + subscribedChannelID },
                { "EventData",                      "Event:" + subscribedChannelID },
                { "Command",                        "Command:" + subscribedChannelID },
                { "InteractionRequest",             "InteractionRequest:" + subscribedChannelID },
                { "InteractionResponse",            "InteractionResponse:" + subscribedChannelID },
                { "ControlRequest",                 "ControlRequest:" + subscribedChannelID },
                { "ControlTransferRequest",         "TransferRequest:" + subscribedChannelID },
                { "ControlLost",                    "ControlLost:" + subscribedChannelID },
                { "ControlReclamation",             "ControlReclamation:" + subscribedChannelID },
                { "ControlRegained",                "ControlRegained:" + subscribedChannelID },
                { "ControlTransferResponse",        "ControlTransferResponse:" + subscribedChannelID },
                { "ControlResponse",                "ControlResponse:" + subscribedChannelID },
                { "ControlRelinquished",            "ControlRelinquished:" + subscribedChannelID },
                { "PrimaryControlRelinquished",     "PrimaryControlRelinquished:" + subscribedChannelID },
                { "SetCurrentController",           "SetCurrentController:" + subscribedChannelID },
                { "PrimaryControlResponse",         "PrimaryControlResponse:" + subscribedChannelID },
                { "PrimaryControlRequest",          "PrimaryControlRequest:" + subscribedChannelID },
                { "ControlGained",                  "ControlGained:" + subscribedChannelID },
            };

            PublishChannels = new Dictionary<string, string>()
            {
                { "ConstructData",                  "ConstructData:" + endpointChannelID },
                { "StateFieldData",                 "StateFieldData:" + endpointChannelID },
                { "PerceptionData",                 "PerceptionData:" + endpointChannelID },
                { "WaypointData",                   "WaypointData:" + endpointChannelID },
                { "EventData",                      "EventData:" + endpointChannelID },
                { "Command",                        "Command:" + endpointChannelID },
                { "InteractionRequest",             "InteractionRequest:" + endpointChannelID },
                { "InteractionResponse",            "InteractionResponse:" + endpointChannelID },
                { "ControlRequest",                 "ControlRequest:" + endpointChannelID },
                { "ControlTransferRequest",         "TransferRequest:" + endpointChannelID },
                { "ControlLost",                    "ControlLost:" + endpointChannelID },
                { "ControlReclamation",             "ControlReclamation:" + endpointChannelID },
                { "ControlRegained",                "ControlRegained:" + endpointChannelID },
                { "ControlTransferResponse",        "ControlTransferResponse:" + endpointChannelID },
                { "ControlResponse",                "ControlResponse:" + endpointChannelID },
                { "ControlRelinquished",            "ControlRelinquished:" + endpointChannelID },
                { "PrimaryControlRelinquished",     "PrimaryControlRelinquished:" + endpointChannelID },
                { "SetCurrentController",           "SetCurrentController:" + endpointChannelID },
                { "PrimaryControlResponse",         "PrimaryControlResponse:" + endpointChannelID },
                { "PrimaryControlRequest",          "PrimaryControlRequest:" + endpointChannelID },
                { "ControlGained",                  "ControlGained:" + endpointChannelID },
            };

            commandActions = new Dictionary<uint, Action<JObject>>()
            {
                {454000002, (json) => { Receive(new TransferConstructIDMessage(json)); } },
                {454004005, (json) => { Receive(new DisplayMessagesMessage(json)); } },
                {454007001, (json) => { Receive(new StopConstructMessage(json)); } },
                {454007002, (json) => { Receive(new RemoveConstructMessage(json)); } },
                {454009000, (json) => { Receive(new RequestSimulationTimeMessage(json)); } },
                {454009001, (json) => { Receive(new SetSimulationTimeMessage(json)); } },
                {454009999, (json) => { Receive(new SimulationTimeMessage(json)); } },
                {454013000, (json) => { Receive(new CreateEnvironmentRequestMessage(json)); } },
                {454013001, (json) => { Receive(new CreateEnvironmentResponseMessage(json)); } },
                {454013002, (json) => { Receive(new CreateConstructRequestMessage(json)); } },
                {454013003, (json) => { Receive(new CreateConstructResponseMessage(json)); } },
                {454013004, (json) => { Receive(new ParameterizeConstructRequestMessage(json)); } },
                {454013005, (json) => { Receive(new ParameterizeConstructResponseMessage(json)); } },
                {454013006, (json) => { Receive(new FinalizeScenarioRequestMessage(json)); } },
                {454013007, (json) => { Receive(new FinalizeScenarioResponseMessage(json)); } },
                {454013008, (json) => { Receive(new ScenarioStartMessage(json)); } },
                {454999001, (json) => { Receive(new ControlInitiatedMessage(json)); } },
                {454999002, (json) => { Receive(new ControlReleasedMessage(json)); } }
            };
        }

        /// <summary>
        /// Subscribes to MUSIC network traffic by using the Redis subscriber to subscribe to MUSIC channels.
        /// Throws an exception if called while already subscribed.
        /// </summary>
        public override void SubscribeToMUSIC()
        {
            if (Subscribed)
            {
                throw new SubscriptionException("Cannot subscribe to MUSIC while already subscribed");
            }

            Subscriber.Subscribe(SubscribeChannels["ConstructData"], (channel, message) =>
            {
                ReceiveMessage(message, (JObject jsonObj) => { return new ConstructDataMessage(jsonObj); });
            });

            Subscriber.Subscribe(SubscribeChannels["StateFieldData"], (channel, message) =>
            {
                ReceiveMessage(message, (JObject jsonObj) => { return new StateFieldMessage(jsonObj); });
            });

            Subscriber.Subscribe(SubscribeChannels["InteractionRequest"], (channel, message) =>
            {
                ReceiveMessage(message, (JObject jsonObj) => { return new InteractionRequestMessage(jsonObj); });
            });

            Subscriber.Subscribe(SubscribeChannels["InteractionResponse"], (channel, message) =>
            {
                ReceiveMessage(message, (JObject jsonObj) => { return new InteractionResponseMessage(jsonObj); });
            });

            Subscriber.Subscribe(SubscribeChannels["ControlRequest"], (channel, message) =>
            {
                ReceiveMessage(message, (JObject jsonObj) => { return new ControlRequestMessage(jsonObj); });

            });

            Subscriber.Subscribe(SubscribeChannels["ControlTransferRequest"], (channel, message) =>
            {
                ReceiveMessage(message, (JObject jsonObj) => { return new ControlTransferRequestMessage(jsonObj); });
            });

            Subscriber.Subscribe(SubscribeChannels["ControlLost"], (channel, message) =>
            {
                ReceiveMessage(message, (JObject jsonObj) => { return new ControlLostMessage(jsonObj); });
            });

            Subscriber.Subscribe(SubscribeChannels["PerceptionData"], (channel, message) =>
            {
                ReceiveMessage(message, (JObject jsonObj) => { return new PerceptionDataMessage(jsonObj); });
            });

            Subscriber.Subscribe(SubscribeChannels["WaypointData"], (channel, message) =>
            {
                ReceiveMessage(message, (JObject jsonObj) => { return new WaypointDataMessage(jsonObj); });
            });

            Subscriber.Subscribe(SubscribeChannels["EventData"], (channel, message) =>
            {
                ReceiveMessage(message, (JObject jsonObj) => { return new MUSICEventMessage(jsonObj); });
            });

            Subscriber.Subscribe(SubscribeChannels["Command"], (channel, message) =>
            {
                JObject commandJSON = JObject.Parse(message);
                commandActions[(uint)commandJSON["commandIdentifier"]](commandJSON);
            });

            Subscriber.Subscribe(SubscribeChannels["ControlReclamation"], (channel, message) =>
            {
                ReceiveMessage(message, (JObject jsonObj) => { return new ControlReclamationMessage(jsonObj); });
            });

            Subscriber.Subscribe(SubscribeChannels["ControlRegained"], (channel, message) =>
            {
                ReceiveMessage(message, (JObject jsonObj) => { return new ControlRegainedMessage(jsonObj); });
            });

            Subscriber.Subscribe(SubscribeChannels["ControlTransferResponse"], (channel, message) =>
            {
                ReceiveMessage(message, (JObject jsonObj) => { return new ControlTransferResponseMessage(jsonObj); });
            });

            Subscriber.Subscribe(SubscribeChannels["ControlResponse"], (channel, message) =>
            {
                ReceiveMessage(message, (JObject jsonObj) => { return new ControlResponseMessage(jsonObj); });
            });

            Subscriber.Subscribe(SubscribeChannels["ControlRelinquished"], (channel, message) =>
            {
                ReceiveMessage(message, (JObject jsonObj) => { return new ControlRelinquishedMessage(jsonObj); });
            });

            Subscriber.Subscribe(SubscribeChannels["PrimaryControlRelinquished"], (channel, message) =>
            {
                ReceiveMessage(message, (JObject jsonObj) => { return new PrimaryControlRelinquishedMessage(jsonObj); });
            });

            Subscriber.Subscribe(SubscribeChannels["SetCurrentController"], (channel, message) =>
            {
                ReceiveMessage(message, (JObject jsonObj) => { return new SetCurrentControllerMessage(jsonObj); });
            });

            Subscriber.Subscribe(SubscribeChannels["PrimaryControlResponse"], (channel, message) =>
            {
                ReceiveMessage(message, (JObject jsonObj) => { return new PrimaryControlResponseMessage(jsonObj); });
            });

            Subscriber.Subscribe(SubscribeChannels["PrimaryControlRequest"], (channel, message) =>
            {
                ReceiveMessage(message, (JObject jsonObj) => { return new PrimaryControlRequestMessage(jsonObj); });
            });

            Subscriber.Subscribe(SubscribeChannels["ControlGained"], (channel, message) =>
            {
                ReceiveMessage(message, (JObject jsonObj) => { return new ControlGainedMessage(jsonObj); });
            });

            Subscribed = true;
        }

        private void ReceiveMessage(string message, MessageInstantiationDel newMessage)
        {
            LastMessage = message;
            Receive(newMessage(JObject.Parse(message)));
        }

        public override void UnsubscribeFromMUSIC()
        {
            Subscriber.UnsubscribeAll();
            Subscribed = false;
        }

        public override void VisitStateField(StateFieldMessage message)
        {
            Subscriber.Publish(PublishChannels["StateFieldData"], message.ToJsonObject().ToString());
        }

        public override void VisitConstructData(ConstructDataMessage message)
        {
            Subscriber.Publish(PublishChannels["ConstructData"], message.ToJsonObject().ToString());
        }

        public override void VisitMUSICEvent(MUSICEventMessage message)
        {
            Subscriber.Publish(PublishChannels["EventData"], message.ToJsonObject().ToString());
        }

        public override void VisitInteractionRequest(InteractionRequestMessage message)
        {
            Subscriber.Publish(PublishChannels["InteractionRequest"], message.ToJsonObject().ToString());
        }

        public override void VisitInteractionResponse(InteractionResponseMessage message)
        {
            Subscriber.Publish(PublishChannels["InteractionResponse"], message.ToJsonObject().ToString());
        }

        public override void VisitControlRequest(ControlRequestMessage message)
        {
            Subscriber.Publish(PublishChannels["ControlRequest"], message.ToJsonObject().ToString());
        }

        public override void VisitControlTransferRequest(ControlTransferRequestMessage message)
        {
            Subscriber.Publish(PublishChannels["ControlTransferRequest"], message.ToJsonObject().ToString());
        }

        public override void VisitCreateConstructRequest(CreateConstructRequestMessage message)
        {
            Subscriber.Publish(PublishChannels["Command"], message.ToJsonObject().ToString());
        }

        public override void VisitCreateEnvironmentRequest(CreateEnvironmentRequestMessage message)
        {
            Subscriber.Publish(PublishChannels["Command"], message.ToJsonObject().ToString());
        }

        public override void VisitFinalizeScenarioRequest(FinalizeScenarioRequestMessage message)
        {
            Subscriber.Publish(PublishChannels["Command"], message.ToJsonObject().ToString());
        }

        public override void VisitParameterizeConstructRequest(ParameterizeConstructRequestMessage message)
        {
            Subscriber.Publish(PublishChannels["Command"], message.ToJsonObject().ToString());
        }

        public override void VisitPrimaryControlRequest(PrimaryControlRequestMessage message)
        {
            Subscriber.Publish(PublishChannels["PrimaryControlRequest"], message.ToJsonObject().ToString());
        }

        public override void VisitControlResponse(ControlResponseMessage message)
        {
            Subscriber.Publish(PublishChannels["ControlResponse"], message.ToJsonObject().ToString());
        }

        public override void VisitControlTransferResponse(ControlTransferResponseMessage message)
        {
            Subscriber.Publish(PublishChannels["ControlTransferResponse"], message.ToJsonObject().ToString());
        }

        public override void VisitCreateConstructResponse(CreateConstructResponseMessage message)
        {
            Subscriber.Publish(PublishChannels["Command"], message.ToJsonObject().ToString());

        }

        public override void VisitCreateEnvironmentResponse(CreateEnvironmentResponseMessage message)
        {
            Subscriber.Publish(PublishChannels["Command"], message.ToJsonObject().ToString());
        }

        public override void VisitFinalizeScenarioResponse(FinalizeScenarioResponseMessage message)
        {
            Subscriber.Publish(PublishChannels["Command"], message.ToJsonObject().ToString());
        }

        public override void VisitParameterizeConstructResponse(ParameterizeConstructResponseMessage message)
        {
            Subscriber.Publish(PublishChannels["Command"], message.ToJsonObject().ToString());
        }

        public override void VisitPrimaryControlResponse(PrimaryControlResponseMessage message)
        {
            Subscriber.Publish(PublishChannels["PrimaryControlResponse"], message.ToJsonObject().ToString());
        }

        public override void VisitControlInitiated(ControlInitiatedMessage message)
        {
            Subscriber.Publish(PublishChannels["Command"], message.ToJsonObject().ToString());
        }

        public override void VisitControlLost(ControlLostMessage message)
        {
            Subscriber.Publish(PublishChannels["ControlLost"], message.ToJsonObject().ToString());
        }

        public override void VisitControlReclamation(ControlReclamationMessage message)
        {
            Subscriber.Publish(PublishChannels["ControlReclamation"], message.ToJsonObject().ToString());
        }

        public override void VisitControlRegained(ControlRegainedMessage message)
        {
            Subscriber.Publish(PublishChannels["ControlRegained"], message.ToJsonObject().ToString());
        }

        public override void VisitControlReleased(ControlReleasedMessage message)
        {
            Subscriber.Publish(PublishChannels["Command"], message.ToJsonObject().ToString());
        }

        public override void VisitControlRelinquished(ControlRelinquishedMessage message)
        {
            Subscriber.Publish(PublishChannels["ControlRelinquished"], message.ToJsonObject().ToString());
        }

        public override void VisitDisplayMessages(DisplayMessagesMessage message)
        {
            Subscriber.Publish(PublishChannels["Command"], message.ToJsonObject().ToString());
        }

        public override void VisitPrimaryControlRelinquished(PrimaryControlRelinquishedMessage message)
        {
            Subscriber.Publish(PublishChannels["PrimaryControlRelinquished"], message.ToJsonObject().ToString());
        }

        public override void VisitRemoveConstruct(RemoveConstructMessage message)
        {
            Subscriber.Publish(PublishChannels["Command"], message.ToJsonObject().ToString());
        }

        public override void VisitRequestSimulationTime(RequestSimulationTimeMessage message)
        {
            Subscriber.Publish(PublishChannels["Command"], message.ToJsonObject().ToString());
        }

        public override void VisitScenarioStart(ScenarioStartMessage message)
        {
            Subscriber.Publish(PublishChannels["Command"], message.ToJsonObject().ToString());
        }

        public override void VisitSetCurrentController(SetCurrentControllerMessage message)
        {
            Subscriber.Publish(PublishChannels["SetCurrentController"], message.ToJsonObject().ToString());
        }

        public override void VisitSetSimulationTime(SetSimulationTimeMessage message)
        {
            Subscriber.Publish(PublishChannels["Command"], message.ToJsonObject().ToString());
        }

        public override void VisitSimulationTime(SimulationTimeMessage message)
        {
            Subscriber.Publish(PublishChannels["Command"], message.ToJsonObject().ToString());
        }

        public override void VisitStopConstruct(StopConstructMessage message)
        {
            Subscriber.Publish(PublishChannels["Command"], message.ToJsonObject().ToString());
        }

        public override void VisitTransferConstructID(TransferConstructIDMessage message)
        {
            Subscriber.Publish(PublishChannels["Command"], message.ToJsonObject().ToString());
        }

        public override void VisitControlGained(ControlGainedMessage message)
        {
            Subscriber.Publish(PublishChannels["ControlGained"], message.ToJsonObject().ToString());
        }

        public override void VisitWaypointData(WaypointDataMessage message)
        {
            Subscriber.Publish(PublishChannels["WaypointData"], message.ToJsonObject().ToString());
        }

        public override void VisitPerceptionData(PerceptionDataMessage message)
        {
            Subscriber.Publish(PublishChannels["PerceptionData"], message.ToJsonObject().ToString());
        }

        public class SubscriptionException : Exception
        {
            public SubscriptionException(String message) : base(message)
            {

            }
        }
    }
}
