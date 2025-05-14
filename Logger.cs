using System;
using System.IO;
using System.Text.Json;

namespace ProjetEasySave
{
    public class Logger
    {
        private readonly string logDirectory;

        public Logger(string logDirectory)
        {
            this.logDirectory = logDirectory;
            if (!Directory.Exists(logDirectory))
                Directory.CreateDirectory(logDirectory);
        }

        public void WriteLogEntry(LogEntry entry)
        {
            string logFile = Path.Combine(logDirectory, $"{DateTime.Now:yyyy-MM-dd}.json");
            string json = JsonSerializer.Serialize(entry);
            File.AppendAllText(logFile, json + Environment.NewLine);
        }
    }
}
