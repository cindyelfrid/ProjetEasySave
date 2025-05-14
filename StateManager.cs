using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ProjetEasySave
{
    public class StateManager
    {
        private readonly string stateFile;

        public StateManager(string stateFile)
        {
            this.stateFile = stateFile;
            var dir = Path.GetDirectoryName(stateFile);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }

        public void UpdateState(JobState jobState)
        {
            List<JobState> states = new List<JobState>();
            if (File.Exists(stateFile))
            {
                try
                {
                    var json = File.ReadAllText(stateFile);
                    states = JsonSerializer.Deserialize<List<JobState>>(json) ?? new List<JobState>();
                }
                catch { }
            }

            var existing = states.FirstOrDefault(s => s.Name == jobState.Name);
            if (existing != null)
                states[states.IndexOf(existing)] = jobState;
            else
                states.Add(jobState);

            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(stateFile, JsonSerializer.Serialize(states, options));
        }
    }
}
