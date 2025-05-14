using System.Text.Json.Serialization;

namespace ProjetEasySave
{
    public class BackupJob
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("SourceDirectory")]
        public string SourceDirectory { get; set; } = string.Empty;

        [JsonPropertyName("TargetDirectory")]
        public string TargetDirectory { get; set; } = string.Empty;

        [JsonPropertyName("Type")]
        public int Type { get; set; } // 0 = Complete, 1 = Differential

        [JsonPropertyName("FileSize")]
        public long FileSize { get; set; }

        [JsonPropertyName("FileTransferTime")]
        public double FileTransferTime { get; set; }

        [JsonPropertyName("Status")]
        public int Status { get; set; } // 0 = Active, 1 = Completed

        [JsonPropertyName("LastAction")]
        public string LastAction { get; set; } = string.Empty;

        // Constructeur pratique pour l'ajout
        public BackupJob(string name, string src, string tgt, int type)
        {
            Name = name;
            SourceDirectory = src;
            TargetDirectory = tgt;
            Type = type;
            FileSize = 0;
            FileTransferTime = 0;
            Status = 0;
            LastAction = "";
        }

        public BackupJob() { }
    }
}
