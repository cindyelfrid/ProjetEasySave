using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ProjetEasySave
{
    class Program
    {
        static BackupManager backupManager = new BackupManager();
        public static Dictionary<string, string> Msg = Messages.Fr; // Par défaut français

        static void Main(string[] args)
        {
            // Multilingue
            Console.WriteLine("Choisissez la langue / Choose language :");
            Console.WriteLine("1. Français");
            Console.WriteLine("2. English");
            Console.Write("Votre choix / Your choice: ");
            string? lang = Console.ReadLine();
            if (lang == "2")
                Msg = Messages.En;

            Console.WriteLine(Msg["Title"]);

            // Lecture config
            AppConfig config;
            try
            {
                config = JsonSerializer.Deserialize<AppConfig>(File.ReadAllText("config.json")) ?? new AppConfig();
            }
            catch
            {
                config = new AppConfig();
            }

            var logger = new Logger(config.LogDirectory);
            var stateManager = new StateManager(config.StateFile);

            backupManager.LoadJobs();

            // Exécution directe par ligne de commande
            if (args.Length > 0)
            {
                var selection = ParseSelection(args[0], backupManager.Jobs.Count);
                backupManager.ExecuteJobs(selection, logger, stateManager);
                return;
            }

            while (true)
            {
                Console.WriteLine(Msg["Menu"]);
                Console.Write(Msg["Choice"]);
                string? choix = Console.ReadLine();

                if (choix == "1")
                    backupManager.AddJob();
                else if (choix == "2")
                    backupManager.ListJobs();
                else if (choix == "3")
                {
                    Console.Write(Msg["EnterIndices"]);
                    string? input = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        Console.WriteLine(Msg["NoSelection"]);
                        continue;
                    }
                    var selection = ParseSelection(input, backupManager.Jobs.Count);
                    backupManager.ExecuteJobs(selection, logger, stateManager);
                }
                else if (choix == "4")
                {
                    var allIndices = Enumerable.Range(0, backupManager.Jobs.Count).ToList();
                    backupManager.ExecuteJobs(allIndices, logger, stateManager);
                }
                else if (choix == "0")
                    break;
                else
                    Console.WriteLine(Msg["InvalidChoice"]);
            }
        }

        static List<int> ParseSelection(string? input, int max)
        {
            var result = new List<int>();
            if (string.IsNullOrWhiteSpace(input))
                return result;

            if (input.Contains("-"))
            {
                var parts = input.Split('-');
                if (int.TryParse(parts[0], out int start) && int.TryParse(parts[1], out int end))
                    for (int i = start - 1; i < end && i < max; i++) result.Add(i);
            }
            else if (input.Contains(";"))
            {
                var parts = input.Split(';');
                foreach (var p in parts)
                    if (int.TryParse(p, out int idx) && idx - 1 < max) result.Add(idx - 1);
            }
            else if (int.TryParse(input, out int idx) && idx - 1 < max)
                result.Add(idx - 1);
            return result;
        }
    }
}
