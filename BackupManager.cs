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
        private readonly string configPath = "jobs.json"; // Peut aussi être paramétré

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
            File.WriteAllText(configPath, JsonSerializer.Serialize(Jobs, new JsonSerializerOptions { WriteIndented = true }));
        }

        public void ListJobs()
        {
            if (Jobs.Count == 0)
            {
                Console.WriteLine(Program.Msg["NoJobs"]);
                return;
            }
            for (int i = 0; i < Jobs.Count; i++)
            {
                string typeStr = Jobs[i].Type == 0 ? Program.Msg["FullType"] : Program.Msg["DiffType"];
                Console.WriteLine($"{i + 1}. {Jobs[i].Name} [{typeStr}] {Jobs[i].SourceDirectory} -> {Jobs[i].TargetDirectory}");
            }
        }

        public void AddJob()
        {
            if (Jobs.Count >= 5)
            {
                Console.WriteLine(Program.Msg["MaxJobs"]);
                return;
            }
            Console.Write(Program.Msg["AddJob"]);
            string? name = Console.ReadLine();
            Console.Write(Program.Msg["SourceDir"]);
            string? src = Console.ReadLine();
            Console.Write(Program.Msg["TargetDir"]);
            string? tgt = Console.ReadLine();
            Console.Write(Program.Msg["TypePrompt"]);
            string? typeInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(src) || string.IsNullOrWhiteSpace(tgt) || string.IsNullOrWhiteSpace(typeInput))
            {
                Console.WriteLine(Program.Msg["AllFieldsRequired"]);
                return;
            }

            if (!int.TryParse(typeInput, out int t) || (t != 1 && t != 2))
            {
                Console.WriteLine(Program.Msg["TypeInvalid"]);
                return;
            }
            int type = t == 2 ? 1 : 0; // 0 = Complète/Full, 1 = Différentielle/Differential
            Jobs.Add(new BackupJob(name, src, tgt, type));
            SaveJobs();
            Console.WriteLine(Program.Msg["JobAdded"]);
        }

        public void ExecuteJobs(List<int> selection, Logger logger, StateManager stateManager)
        {
            foreach (var idx in selection)
            {
                if (idx < 0 || idx >= Jobs.Count) continue;
                var job = Jobs[idx];

                if (!Directory.Exists(job.SourceDirectory))
                {
                    Console.WriteLine(string.Format(Program.Msg["SourceMissing"], job.SourceDirectory));
                    continue;
                }
                if (!Directory.Exists(job.TargetDirectory))
                {
                    Console.WriteLine(string.Format(Program.Msg["TargetMissing"], job.TargetDirectory));
                    continue;
                }

                var files = Directory.GetFiles(job.SourceDirectory, "*", SearchOption.AllDirectories);
                int totalFiles = files.Length;
                long totalSize = files.Sum(f => new FileInfo(f).Length);

                for (int i = 0; i < files.Length; i++)
                {
                    string file = files[i];
                    string relPath = Path.GetRelativePath(job.SourceDirectory, file);
                    string destFile = Path.Combine(job.TargetDirectory, relPath);
                    string? destDir = Path.GetDirectoryName(destFile);
                    if (!string.IsNullOrEmpty(destDir))
                        Directory.CreateDirectory(destDir);

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
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(string.Format(Program.Msg["CopyError"], file, ex.Message));
                    }

                    // Mise à jour de l'état temps réel
                    int filesLeft = totalFiles - (i + 1);
                    int progression = totalFiles > 0 ? (int)(((double)(i + 1) / totalFiles) * 100) : 0;
                    stateManager.UpdateState(new JobState
                    {
                        Name = job.Name,
                        SourceFilePath = file,
                        TargetFilePath = destFile,
                        State = "ACTIVE",
                        TotalFilesToCopy = totalFiles,
                        TotalFilesSize = totalSize,
                        NbFilesLeftToDo = filesLeft,
                        Progression = progression
                    });

                    // Log (optionnel)
                    logger.WriteLogEntry(new
                    {
                        Name = job.Name,
                        FileSource = file,
                        FileTarget = destFile,
                        FileSize = new FileInfo(file).Length,
                        Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                    });
                }

                // Quand le job est terminé
                stateManager.UpdateState(new JobState
                {
                    Name = job.Name,
                    SourceFilePath = "",
                    TargetFilePath = "",
                    State = "END",
                    TotalFilesToCopy = 0,
                    TotalFilesSize = 0,
                    NbFilesLeftToDo = 0,
                    Progression = 100
                });
            }
            Console.WriteLine(Program.Msg["BackupCompleted"]);
        }
    }
}
