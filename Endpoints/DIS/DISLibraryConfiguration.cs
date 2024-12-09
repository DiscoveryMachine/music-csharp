//=====================================================================
// DISCOVERY MACHINE, INC. PROPRIETARY INFORMATION
//
// This software is supplied under the terms of a license agreement
// or nondisclosure agreement with Discovery Machine, Inc. and may
// not be copied or disclosed except in accordance with the terms of
// that agreement.
//
// Copyright 2023 Discovery Machine, Inc. All Rights Reserved.
//=====================================================================

using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Sockets;

namespace MUSICLibrary.Endpoints.DIS
{
    public class DISLibraryConfiguration : LibraryConfiguration
    {
        private JObject disConfig;
        private string hostAddress;
        private int receivePort;
        private int sendPort;

        private UdpClient _receiverClient;
        public UdpClient ReceiverClient
        {
            get
            {
                lock (_receiverClient)
                    while (ReceiverIsDown(_receiverClient))
                        CreateReceiverClient();
                return _receiverClient;
            }
            private set { _receiverClient = value; }
        }

        private UdpClient _senderClient;
        public UdpClient SenderClient
        {
            get
            {
                lock(_senderClient)
                    while (SenderIsDown(_senderClient))
                        CreateSenderClient();
                return _senderClient;
            }
            private set { _senderClient = value; }
        }

        public override void ReadFromFile()
        {
            base.ReadFromFile();
            disConfig = jsonConfig["endpoint_config"]["dis"] as JObject;
            hostAddress = disConfig["host"].ToObject<string>();
            CreateReceiverClient();
            CreateSenderClient();
        }

        public IPEndPoint CreateRemoteReceiverEndpoint()
        {
            return new IPEndPoint(IPAddress.Any, receivePort);
        }

        private void CreateReceiverClient()
        {
            receivePort = disConfig["receive_port"].ToObject<int>();

            _receiverClient = InitializeUdpClient();
            _receiverClient.Client.ReceiveTimeout = 5000; //Keep this timeout above 1 second. Otherwise, it may cause receiver tasks to not work properly.
            _receiverClient.Client.Bind(new IPEndPoint(IPAddress.Any, receivePort));
            _receiverClient.Client.ReceiveBufferSize = int.MaxValue;
        }

        private void CreateSenderClient()
        {
            sendPort = disConfig["send_port"].ToObject<int>();
            var sendAddress = new IPEndPoint(IPAddress.Parse(hostAddress), sendPort);

            _senderClient = InitializeUdpClient();
            _senderClient.Client.SendTimeout = 5000;
            _senderClient.Connect(sendAddress);
        }

        private UdpClient InitializeUdpClient()
        {
            var udp = new UdpClient
            {
                ExclusiveAddressUse = false,
                DontFragment = true,
                MulticastLoopback = false,
                EnableBroadcast = false,
            };
            
            udp.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            return udp;
        }

        private bool SenderIsDown(UdpClient udpClient)
        {
            try
            {
                return
                    udpClient == null ||
                    udpClient.Client == null ||
                    !udpClient.Client.Connected;
                
            }
            catch(Exception e)
            {
                Logs.Instance.Log(e.Message + "\n" + e.StackTrace);
                //In the event of literally anything going wrong accessing the socket, assume the udp client is dead.
                return true;
            }
        }

        private bool ReceiverIsDown(UdpClient udpClient)
        {
            try
            {
                return
                    udpClient == null ||
                    udpClient.Client == null ||
                    !udpClient.Client.IsBound;
            }
            catch (Exception e)
            {
                Logs.Instance.Log(e.Message + "\n" + e.StackTrace);
                //In the event of literally anything going wrong accessing the socket, assume the udp client is dead.
                return true;
            }
        }
    }
}
