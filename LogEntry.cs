using System.Text.Json.Serialization;

namespace ProjetEasySave
{
    public class LogEntry
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("FileSource")]
        public string FileSource { get; set; } = string.Empty;

        [JsonPropertyName("FileTarget")]
        public string FileTarget { get; set; } = string.Empty;

        [JsonPropertyName("FileSize")]
        public long FileSize { get; set; }

        [JsonPropertyName("FileTransferTime")]
        public double FileTransferTime { get; set; }

        [JsonPropertyName("time")]
        public string Time { get; set; } = string.Empty;
    }
}
