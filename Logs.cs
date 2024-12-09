using System;
using System.IO;

namespace MUSICLibrary
{
    public class Logs
    {
        private static Logs _instance;
        private static readonly string configPath = 
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
            @"\DiscoveryMachine\dm\MUSICLibraryLogs.txt";

        private readonly string logPath;
        private readonly object _lock;
        private readonly bool enabled = true;

        public static Logs Instance 
        { 
            get
            {
                if (_instance == null)
                    _instance = new Logs(configPath);
                return _instance;
            }
            private set { }
        }

        private Logs(string logPath)
        {
            this.logPath = logPath;
            _lock = new object();
            if (!File.Exists(logPath))
                File.Create(logPath);
        }

        public void Log(string message)
        {
            Log("Log", message);
        }

        public void LogError(string message)
        {
            Log("Error", message);
        }

        public void LogWarning(string message)
        {
            Log("Warning", message);
        }

        private void Log(string type, string message)
        {
            if (enabled)
                lock (_lock)
                    File.AppendAllText(logPath, "[" + type + "]<" + DateTime.Now.ToString("HH:mm:ss") + "> " + message + "\n\n");
        }
    }
}