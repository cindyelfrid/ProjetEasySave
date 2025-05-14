using System;
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
        }

        /// <summary>
        /// Met à jour ou ajoute l'état d'un travail dans le fichier state.json.
        /// </summary>
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
                catch { /* Fichier corrompu ou vide, on repart à zéro */ }
            }

            // Remplace ou ajoute l'état du job
            var existing = states.FirstOrDefault(s => s.Name == jobState.Name);
            if (existing != null)
            {
                int idx = states.IndexOf(existing);
                states[idx] = jobState;
            }
            else
            {
                states.Add(jobState);
            }

            File.WriteAllText(stateFile, JsonSerializer.Serialize(states, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}
