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

using Newtonsoft.Json.Linq;
using System.Net;

namespace MUSICLibrary.Endpoints.Redis
{
    public class RedisLibraryConfiguration : LibraryConfiguration
    {
        public IPEndPoint RedisHost { get; set; }
        public string Password { get; set; }

        public override void ReadFromFile()
        {
            base.ReadFromFile();

            var redisConfig = jsonConfig["endpoint_config"]["redis"] as JObject;

            RedisHost = new IPEndPoint(
                IPAddress.Parse(redisConfig["host"].ToObject<string>()),
                redisConfig["port"].ToObject<int>()
            );

            Password = redisConfig["password"].ToObject<string>();
        }
    }
}
