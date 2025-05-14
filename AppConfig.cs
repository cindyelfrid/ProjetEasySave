using System.Text.Json.Serialization;

namespace ProjetEasySave
{
    public class AppConfig
    {
        [JsonPropertyName("LogDirectory")]
        public string LogDirectory { get; set; } = ".\\Data\\Logs\\";

        [JsonPropertyName("StateFile")]
        public string StateFile { get; set; } = ".\\Data\\state.json";
    }
}
