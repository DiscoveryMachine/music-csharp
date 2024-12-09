//=====================================================================
// DISCOVERY MACHINE, INC. PROPRIETARY INFORMATION
//
// This software is supplied under the terms of a license agreement
// or nondisclosure agreement with Discovery Machine, Inc. and may
// not be copied or disclosed except in accordance with the terms  of
// that agreement.
//
// Copyright 2023 Discovery Machine, Inc. All Rights Reserved.
//=====================================================================

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using MUSICLibrary.Endpoints.DIS.DIS_PDU_Makers;
using MUSICLibrary.Endpoints.DIS.Factories;
using MUSICLibrary.MUSIC_Messages;
using MUSICLibrary.MUSIC_Messages.Construct_Data;
using MUSICLibrary.MUSIC_Messages.MUSIC_Request_Messages;
using MUSICLibrary.MUSIC_Messages.MUSIC_Response_Messages;
using MUSICLibrary.MUSIC_Messages.Targeted_MUSIC_Messages;

namespace MUSICLibrary.Endpoints.DIS
{
    // This class currently has no unit tests associated with it.
    // Unit testing with sockets over multiple threads is difficult to impossible
    // therefore, this class will need to be manually tested.

    public class DISEndpoint : MUSICEndpoint
    {
        private readonly DISLibraryConfiguration config;
        private readonly Dictionary<byte, IDISMUSICMessageFactory> pduTypeFactories;
        private readonly Dictionary<long, IDISMUSICMessageFactory> variableDatumIDFactories;
        private readonly ConstructDataPduMaker constructDataPduMaker;
        private readonly StateFieldPduMaker stateFieldPduMaker;
        private readonly InteractionRequestPduMaker interactionRequestPduMaker;
        private readonly InteractionResponsePduMaker interactionResponsePduMaker;
        private readonly SimulationTimePduMaker simulationTimePduMaker;

        private readonly BlockingCollection<(PduMaker, MUSICMessage)> outgoingMessageQueue;
        private readonly BlockingCollection<byte[]> incomingPdus;

        private Task receiveThread;
        private Task sendThread;
        private Task processingThread;

        public DISEndpoint(DISLibraryConfiguration config) : base(config)
        {
            this.config = config;
            constructDataPduMaker = new ConstructDataPduMaker();
            stateFieldPduMaker = new StateFieldPduMaker();
            interactionRequestPduMaker = new InteractionRequestPduMaker();
            interactionResponsePduMaker = new InteractionResponsePduMaker();
            simulationTimePduMaker = new SimulationTimePduMaker();

            outgoingMessageQueue = new BlockingCollection<(PduMaker, MUSICMessage)>(new ConcurrentQueue<(PduMaker, MUSICMessage)>());
            incomingPdus = new BlockingCollection<byte[]>(new ConcurrentQueue<byte[]>());

            pduTypeFactories = new Dictionary<byte, IDISMUSICMessageFactory>()
            {
                { 1, new ConstructDataMessageFactory() },
                { 230, new ConstructDataMessageFactory() },

                { 232, new PerceptionDataMessageFactory() },

                { 233, new StateFieldMessageFactory() },

                { 234, new WaypointDataMessageFactory() },
            };
            variableDatumIDFactories = new Dictionary<long, IDISMUSICMessageFactory>()
            {
                { 454110000, new InteractionRequestMessageFactory() },
                { 454110001, new InteractionRequestMessageFactory() },
                { 454110002, new InteractionRequestMessageFactory() },
                { 454110003, new InteractionRequestMessageFactory() },
                { 454110004, new InteractionRequestMessageFactory() },

                { 454119000, new InteractionResponseMessageFactory() },

                { 454009000, new RequestSimulationTimeMessageFactory() },
            };
        }

        public override void SubscribeToMUSIC()
        {
            Subscribed = true;

            processingThread = Task.Run(() => {
                try
                {
                    while (Subscribed)
                    {
                        MUSICMessage message = null;
                        byte[] pdu = incomingPdus.Take();
                        Task.Run(() => {
                            try
                            {
                                message = PduToMUSICMessage(pdu);
                                if (message != null)
                                    Receive(message);
                            }
                            catch (KeyNotFoundException e)
                            {
                                //Ignore KeyNotFoundException because it is likely a state field message which
                                // was received before the construct data of the construct.
                                //For the HLA implementation, this can not be ignored.
                                Logs.Instance.LogError(e.Message + "\n" + e.StackTrace);
                            }
                            catch (Exception e)
                            {
                                //Something unexpected occurred. The endpoint shouldn't have to unsubscribe because
                                //the udp clients *should* still be functioning.
                                Logs.Instance.LogError(e.Message + "\n" + e.StackTrace);
                            }
                        });
                    }
                }
                catch (Exception e) 
                {
                    Logs.Instance.LogError("Processing Thread: " + e.Message + "\n" + e.StackTrace);
                }
            });

            sendThread = Task.Run(() => {
                while (Subscribed)
                {
                    try
                    {
                        var makerAndMsg = outgoingMessageQueue.Take();
                        byte[] pdu = makerAndMsg.Item1.MakeRaw(makerAndMsg.Item2);
                        config.SenderClient.Send(pdu, pdu.Length);
                    }
                    catch (Exception e) 
                    { 
                        Logs.Instance.LogError("Send Thread: " + e.Message + "\n" + e.StackTrace);  
                    } 
                }
            });

            receiveThread = Task.Run(() =>
            {
                IPEndPoint remoteEndpoint = config.CreateRemoteReceiverEndpoint();
                while (Subscribed)
                {
                    try
                    {
                        var pdu = config.ReceiverClient.Receive(ref remoteEndpoint);

                        if (pdu != null)
                            incomingPdus.Add(pdu);
                    }
                    catch (SocketException e) {} //no-op because it is likely just Receive() timing out.
                    catch (Exception e)
                    {
                        Logs.Instance.LogError("Receive Thread: " + e.Message + "\n" + e.StackTrace);
                    }
                }
            });
        }

        private MUSICMessage PduToMUSICMessage(byte[] pdu)
        { 
            var factory = GetMessageFactory(pdu);

            if (factory != null)
                return factory.Create(pdu);

            return null;
        }

        private IDISMUSICMessageFactory GetMessageFactory(byte[] pdu)
        {
            PduReader reader = new PduReader(pdu);

            if (pduTypeFactories.ContainsKey(reader.ReadPduType()))
                return pduTypeFactories[reader.ReadPduType()];
            else if (variableDatumIDFactories.ContainsKey(reader.ReadFirstVariableDatumID()))
                return variableDatumIDFactories[reader.ReadFirstVariableDatumID()];
            else
                return null;
        }

        public override async void UnsubscribeFromMUSIC()
        {
            Subscribed = false;
            config.ReceiverClient.Close();
            config.SenderClient.Close();

            await receiveThread;
            await sendThread;
            await processingThread;
        }
        
        private void TransmitPdu(PduMaker pduMaker, MUSICMessage message)
        {
            outgoingMessageQueue.Add((pduMaker, message));
        }

        public override void VisitConstructData(ConstructDataMessage message)
        {
            bool isGhostedConstruct = 
                message.ConstructRender == ConstructRender.GhostedConstruct ||
                message.ConstructRender == ConstructRender.GhostedLegacy;

            bool hasNoGhostedID = message.GhostedID == null;

            if (isGhostedConstruct && hasNoGhostedID)
                return;

            TransmitPdu(constructDataPduMaker, message);
        }

        public override void VisitStateField(StateFieldMessage message)
        {
            TransmitPdu(stateFieldPduMaker, message);
        }

        public override void VisitInteractionRequest(InteractionRequestMessage message)
        {
            TransmitPdu(interactionRequestPduMaker, message);
        }

        public override void VisitSimulationTime(SimulationTimeMessage message)
        {
            TransmitPdu(simulationTimePduMaker, message);
        }

        public override void VisitInteractionResponse(InteractionResponseMessage message)
        {
            TransmitPdu(interactionResponsePduMaker, message);
        }

        public override void VisitControlGained(ControlGainedMessage message)
        {
            throw new NotImplementedException();
        }

        public override void VisitControlInitiated(ControlInitiatedMessage message)
        {
            throw new NotImplementedException();
        }

        public override void VisitControlLost(ControlLostMessage message)
        {
            throw new NotImplementedException();
        }

        public override void VisitControlReclamation(ControlReclamationMessage message)
        {
            throw new NotImplementedException();
        }

        public override void VisitControlRegained(ControlRegainedMessage message)
        {
            throw new NotImplementedException();
        }

        public override void VisitControlReleased(ControlReleasedMessage message)
        {
            throw new NotImplementedException();
        }

        public override void VisitControlRelinquished(ControlRelinquishedMessage message)
        {
            throw new NotImplementedException();
        }

        public override void VisitControlRequest(ControlRequestMessage message)
        {
            throw new NotImplementedException();
        }

        public override void VisitControlResponse(ControlResponseMessage message)
        {
            throw new NotImplementedException();
        }

        public override void VisitControlTransferRequest(ControlTransferRequestMessage message)
        {
            throw new NotImplementedException();
        }

        public override void VisitControlTransferResponse(ControlTransferResponseMessage message)
        {
            throw new NotImplementedException();
        }

        public override void VisitCreateConstructRequest(CreateConstructRequestMessage message)
        {
            throw new NotImplementedException();
        }

        public override void VisitCreateConstructResponse(CreateConstructResponseMessage message)
        {
            throw new NotImplementedException();
        }

        public override void VisitCreateEnvironmentRequest(CreateEnvironmentRequestMessage message)
        {
            throw new NotImplementedException();
        }

        public override void VisitCreateEnvironmentResponse(CreateEnvironmentResponseMessage message)
        {
            throw new NotImplementedException();
        }

        public override void VisitDisplayMessages(DisplayMessagesMessage message)
        {
            throw new NotImplementedException();
        }

        public override void VisitFinalizeScenarioRequest(FinalizeScenarioRequestMessage message)
        {
            throw new NotImplementedException();
        }

        public override void VisitFinalizeScenarioResponse(FinalizeScenarioResponseMessage message)
        {
            throw new NotImplementedException();
        }

        public override void VisitMUSICEvent(MUSICEventMessage message)
        {
            throw new NotImplementedException();
        }

        public override void VisitParameterizeConstructRequest(ParameterizeConstructRequestMessage message)
        {
            throw new NotImplementedException();
        }

        public override void VisitParameterizeConstructResponse(ParameterizeConstructResponseMessage message)
        {
            throw new NotImplementedException();
        }

        public override void VisitPerceptionData(PerceptionDataMessage message)
        {
            throw new NotImplementedException();
        }

        public override void VisitPrimaryControlRelinquished(PrimaryControlRelinquishedMessage message)
        {
            throw new NotImplementedException();
        }

        public override void VisitPrimaryControlRequest(PrimaryControlRequestMessage message)
        {
            throw new NotImplementedException();
        }

        public override void VisitPrimaryControlResponse(PrimaryControlResponseMessage message)
        {
            throw new NotImplementedException();
        }

        public override void VisitRemoveConstruct(RemoveConstructMessage message)
        {
            throw new NotImplementedException();
        }

        public override void VisitRequestSimulationTime(RequestSimulationTimeMessage message)
        {
            throw new NotImplementedException();
        }

        public override void VisitScenarioStart(ScenarioStartMessage message)
        {
            throw new NotImplementedException();
        }

        public override void VisitSetCurrentController(SetCurrentControllerMessage message)
        {
            throw new NotImplementedException();
        }

        public override void VisitSetSimulationTime(SetSimulationTimeMessage message)
        {
            throw new NotImplementedException();
        }

        public override void VisitStopConstruct(StopConstructMessage message)
        {
            throw new NotImplementedException();
        }

        public override void VisitTransferConstructID(TransferConstructIDMessage message)
        {
            throw new NotImplementedException();
        }

        public override void VisitWaypointData(WaypointDataMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
