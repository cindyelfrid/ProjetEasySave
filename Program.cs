using System;
using System.Collections.Generic;

namespace ProjetEasySave
{
    class Program
    {
        static BackupManager backupManager = new BackupManager();
        static Logger logger = new Logger(@"C:\ProgramData\ProSoft\EasySave\Logs\");
        static StateManager stateManager = new StateManager(@"C:\ProgramData\ProSoft\EasySave\state.json");

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== EasySave v1.0 ===");

            backupManager.LoadJobs();

            if (args.Length > 0)
            {
                var selection = ParseSelection(args[0], backupManager.Jobs.Count);
                backupManager.ExecuteJobs(selection, logger, stateManager);
                Console.WriteLine("Sauvegarde(s) terminée(s).");
                return;
            }

            while (true)
            {
                Console.WriteLine("\n1. Ajouter un travail\n2. Lister les travaux\n3. Exécuter la sauvegarde\n0. Quitter");
                Console.Write("Votre choix : ");
                string? choix = Console.ReadLine();

                if (choix == "1")
                    backupManager.AddJob();
                else if (choix == "2")
                    backupManager.ListJobs();
                else if (choix == "3")
                {
                    Console.Write("Entrez les indices (ex : 1-3 ou 1;3) : ");
                    string? input = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        Console.WriteLine("Aucune sélection saisie.");
                        continue;
                    }
                    var selection = ParseSelection(input, backupManager.Jobs.Count);
                    backupManager.ExecuteJobs(selection, logger, stateManager);
                }
                else if (choix == "0")
                    break;
                else
                    Console.WriteLine("Choix invalide.");
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
