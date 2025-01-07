/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

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
