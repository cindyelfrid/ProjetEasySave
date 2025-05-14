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
        public int Type { get; set; } // 0 = Complète/Full, 1 = Différentielle/Differential

        public BackupJob() { }

        public BackupJob(string name, string src, string tgt, int type)
        {
            Name = name;
            SourceDirectory = src;
            TargetDirectory = tgt;
            Type = type;
        }
    }
}
