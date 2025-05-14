using System.Text.Json.Serialization;

namespace ProjetEasySave
{
    public class JobState
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("SourceFilePath")]
        public string SourceFilePath { get; set; } = string.Empty;

        [JsonPropertyName("TargetFilePath")]
        public string TargetFilePath { get; set; } = string.Empty;

        [JsonPropertyName("State")]
        public string State { get; set; } = string.Empty; // "ACTIVE", "END", etc.

        [JsonPropertyName("TotalFilesToCopy")]
        public int TotalFilesToCopy { get; set; }

        [JsonPropertyName("TotalFilesSize")]
        public long TotalFilesSize { get; set; }

        [JsonPropertyName("NbFilesLeftToDo")]
        public int NbFilesLeftToDo { get; set; }

        [JsonPropertyName("Progression")]
        public int Progression { get; set; } // Pourcentage (0-100)
    }
}
