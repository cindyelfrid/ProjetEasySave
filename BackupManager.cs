using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ProjetEasySave
{
    public class BackupManager
    {
        public List<BackupJob> Jobs { get; set; } = new List<BackupJob>();
        private readonly string configPath = @"C:\ProgramData\ProSoft\EasySave\Config\jobs.json";

        public void LoadJobs()
        {
            if (File.Exists(configPath))
            {
                var jobs = JsonSerializer.Deserialize<List<BackupJob>>(File.ReadAllText(configPath));
                Jobs = jobs ?? new List<BackupJob>();
            }
        }

        public void SaveJobs()
        {
            var dir = Path.GetDirectoryName(configPath);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);
            File.WriteAllText(configPath, JsonSerializer.Serialize(Jobs, new JsonSerializerOptions { WriteIndented = true }));
        }

        public void ListJobs()
        {
            if (Jobs.Count == 0)
            {
                Console.WriteLine("Aucun travail de sauvegarde.");
                return;
            }
            for (int i = 0; i < Jobs.Count; i++)
            {
                string typeStr = Jobs[i].Type == 0 ? "Complète" : "Différentielle";
                string statusStr = Jobs[i].Status == 0 ? "Active" : "Terminée";
                Console.WriteLine($"{i + 1}. {Jobs[i].Name} [{typeStr}] {Jobs[i].SourceDirectory} -> {Jobs[i].TargetDirectory} | Statut: {statusStr}");
            }
        }

        public void AddJob()
        {
            if (Jobs.Count >= 5)
            {
                Console.WriteLine("Nombre maximum de travaux atteint.");
                return;
            }
            Console.Write("Nom du travail : ");
            string? name = Console.ReadLine();
            Console.Write("Répertoire source : ");
            string? src = Console.ReadLine();
            Console.Write("Répertoire cible : ");
            string? tgt = Console.ReadLine();
            Console.Write("Type (1=Complète, 2=Différentielle) : ");
            string? typeInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(src) || string.IsNullOrWhiteSpace(tgt) || string.IsNullOrWhiteSpace(typeInput))
            {
                Console.WriteLine("Erreur : toutes les informations sont obligatoires.");
                return;
            }

            if (!int.TryParse(typeInput, out int t) || (t != 1 && t != 2))
            {
                Console.WriteLine("Type invalide.");
                return;
            }
            int type = t == 2 ? 1 : 0; // 0 = Complète, 1 = Différentielle
            Jobs.Add(new BackupJob(name, src, tgt, type));
            SaveJobs();
            Console.WriteLine("Travail ajouté.");
        }

        public void ExecuteJobs(List<int> selection, Logger logger, StateManager stateManager)
        {
            foreach (var idx in selection)
            {
                if (idx < 0 || idx >= Jobs.Count) continue;
                var job = Jobs[idx];

                // Vérification de l'existence du dossier source
                if (!Directory.Exists(job.SourceDirectory))
                {
                    Console.WriteLine($"Le dossier source '{job.SourceDirectory}' n'existe pas. Sauvegarde ignorée.");
                    continue;
                }

                // Vérification de l'existence du dossier cible
                if (!Directory.Exists(job.TargetDirectory))
                {
                    Console.WriteLine($"Le dossier cible '{job.TargetDirectory}' n'existe pas. Sauvegarde ignorée.");
                    continue;
                }

                job.Status = 0; // Active
                var files = Directory.GetFiles(job.SourceDirectory, "*", SearchOption.AllDirectories);
                job.FileSize = files.Sum(f => new FileInfo(f).Length);
                job.FileTransferTime = 0;
                job.LastAction = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                foreach (var file in files)
                {
                    string? relPath = Path.GetRelativePath(job.SourceDirectory, file);
                    string destFile = Path.Combine(job.TargetDirectory, relPath);
                    string? destDir = Path.GetDirectoryName(destFile);
                    if (!string.IsNullOrEmpty(destDir))
                        Directory.CreateDirectory(destDir);

                    var sw = System.Diagnostics.Stopwatch.StartNew();
                    try
                    {
                        // Différentiel : ne copie que si modifié ou absent
                        if (job.Type == 1 && File.Exists(destFile))
                        {
                            var srcInfo = new FileInfo(file);
                            var tgtInfo = new FileInfo(destFile);
                            if (srcInfo.Length == tgtInfo.Length && srcInfo.LastWriteTime == tgtInfo.LastWriteTime)
                                continue;
                        }
                        File.Copy(file, destFile, true);
                        sw.Stop();
                        double fileTransferTime = sw.ElapsedMilliseconds;
                        job.FileTransferTime += fileTransferTime;
                        job.LastAction = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                        logger.WriteLogEntry(new LogEntry
                        {
                            Name = job.Name,
                            FileSource = file,
                            FileTarget = destFile,
                            FileSize = new FileInfo(file).Length,
                            FileTransferTime = fileTransferTime,
                            Time = job.LastAction
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erreur lors de la copie de '{file}' : {ex.Message}");
                        logger.WriteLogEntry(new LogEntry
                        {
                            Name = job.Name,
                            FileSource = file,
                            FileTarget = destFile,
                            FileSize = new FileInfo(file).Length,
                            FileTransferTime = -1,
                            Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                        });
                    }
                    stateManager.UpdateState(job, files.Length, job.FileSize, 0, 0, file, destFile);
                }
                job.Status = 1; // Completed
                job.LastAction = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                SaveJobs();
            }
        }
    }
}
