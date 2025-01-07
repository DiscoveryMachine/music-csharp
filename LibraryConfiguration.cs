/**
 * Copyright (c)  Discovery Machine®, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using MUSICLibrary.Interfaces;
using MUSICLibrary.MUSIC_Messages;
using MUSICLibrary.Visitors.Default_Internal_Message_Visitor.Construct_Repository;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace MUSICLibrary
{
    public abstract class LibraryConfiguration
    {
        public SiteAndAppID SiteAndAppID { get; set; }
        public uint ExerciseID { get; set; }
        public IMessageFilter MessageFilter { get; set; }
        public IInternalMessageVisitor InternalMessageVisitor { get; set; }
        public IMUSICMessageVisitor ExternalMessageVisitor { get; set; }
        public RepositoryConfiguration RepositoryConfig;

        protected string configPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
            @"\DiscoveryMachine\dm\MUSICLibraryConfig.json";
        protected JObject jsonConfig;

        private const string LOCALHOST = "127.0.0.1";
        
        //These const strings are the keys used by the configuration file.
        private const string LIBRARY_ID = "library_id";
        private const string EXERCISE_ID = "exercise_id";

        private const string ENDPOINT_CONFIG = "endpoint_config";
        private const string REDIS = "redis";
        private const string HOST = "host";
        private const string PORT = "port";
        private const string PASSWORD = "password";
        private const string DIS = "dis";
        private const string SEND_PORT = "send_port";
        private const string RECEIVE_PORT = "receive_port";

        private const string REPO_CONFIG = "repository_config";
        private const string LOCAL_BATCH_COUNT = "local_batch_count";
        private const string REMOTE_BATCH_COUNT = "remote_batch_count";
        private const string LOCAL_BATCH_INTERVAL = "local_batch_interval";
        private const string REMOTE_BATCH_INTERVAL = "remote_batch_interval";
        private const string DEAD_RECKONING_FPS = "dead_reckoning_fps";

        public virtual void ReadFromFile()
        {
            if (!File.Exists(configPath))
                GenerateConfiguration();

            jsonConfig = JObject.Parse(File.ReadAllText(configPath));
            SiteAndAppID = jsonConfig[LIBRARY_ID].ToObject<SiteAndAppID>();
            ExerciseID = jsonConfig[EXERCISE_ID].ToObject<uint>();

            RepositoryConfig = ReadRepositoryConfiguration(jsonConfig[REPO_CONFIG] as JObject);
        }

        private RepositoryConfiguration ReadRepositoryConfiguration(JObject repoConfig)
        {
            var result = new RepositoryConfiguration();

            result.LocalBatchCount = repoConfig[LOCAL_BATCH_COUNT].ToObject<uint>();
            result.RemoteBatchCount = repoConfig[REMOTE_BATCH_COUNT].ToObject<uint>();
            result.LocalBatchInterval = repoConfig[LOCAL_BATCH_INTERVAL].ToObject<uint>();
            result.RemoteBatchInterval = repoConfig[REMOTE_BATCH_INTERVAL].ToObject<uint>();

            return result;
        }

        protected void GenerateConfiguration()
        {
            JObject config = new JObject();
            config[LIBRARY_ID] = new SiteAndAppID(177, 277).ToJsonObject();
            config[EXERCISE_ID] = 1;
            config[REPO_CONFIG] = GenerateDefaultRepositoryConfig();
            config[ENDPOINT_CONFIG] = GenerateDefaultEndpointConfig();

            using (StreamWriter writer = new StreamWriter(File.Create(configPath)))
                writer.WriteLine(config.ToString());
        }

        protected JObject GenerateDefaultEndpointConfig()
        {
            JObject config = new JObject();

            config[REDIS] = GenerateDefaultRedisEndpointConfig();
            config[DIS] = GenerateDefaultDisEndpointConfig();

            return config;
        }

        protected JObject GenerateDefaultDisEndpointConfig()
        {
            JObject config = new JObject();

            config[HOST] = LOCALHOST;
            config[SEND_PORT] = 7777;
            config[RECEIVE_PORT] = 7778;

            return config;
        }

        protected JObject GenerateDefaultRedisEndpointConfig()
        {
            JObject config = new JObject();

            config[HOST] = LOCALHOST;
            config[PORT] = 6379;
            config[PASSWORD] = "";

            return config;
        }

        protected JObject GenerateDefaultRepositoryConfig()
        {
            JObject config = new JObject();

            config[LOCAL_BATCH_COUNT] = 1;
            config[REMOTE_BATCH_COUNT] = 1;
            config[LOCAL_BATCH_INTERVAL] = 5;
            config[REMOTE_BATCH_INTERVAL] = 5;
            config[DEAD_RECKONING_FPS] = 30;

            return config;
        }
    }
}
