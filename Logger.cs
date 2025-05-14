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

        public void WriteLogEntry(object entry)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string logFile = Path.Combine(logDirectory, $"{DateTime.Now:yyyy-MM-dd}.json");
            File.AppendAllText(logFile, JsonSerializer.Serialize(entry, options) + Environment.NewLine);
        }
    }
}
