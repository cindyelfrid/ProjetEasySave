using System.Collections.Generic;

namespace ProjetEasySave
{
    public static class Messages
    {
        public static Dictionary<string, string> Fr = new Dictionary<string, string>
        {
            ["Title"] = "=== EasySave v1.0 ===",
            ["Menu"] = "\n1. Ajouter un travail\n2. Lister les travaux\n3. Exécuter une sauvegarde\n4. Exécuter toutes les sauvegardes\n0. Quitter",
            ["Choice"] = "Votre choix : ",
            ["NoSelection"] = "Aucune sélection saisie.",
            ["InvalidChoice"] = "Choix invalide.",
            ["EnterIndices"] = "Entrez les indices (ex : 1-3 ou 1;3) : ",
            ["BackupCompleted"] = "Sauvegarde(s) terminée(s).",
            ["AddJob"] = "Nom du travail : ",
            ["SourceDir"] = "Répertoire source : ",
            ["TargetDir"] = "Répertoire cible : ",
            ["TypePrompt"] = "Type (1=Complète, 2=Différentielle) : ",
            ["AllFieldsRequired"] = "Erreur : toutes les informations sont obligatoires.",
            ["TypeInvalid"] = "Type invalide.",
            ["MaxJobs"] = "Nombre maximum de travaux atteint.",
            ["JobAdded"] = "Travail ajouté.",
            ["NoJobs"] = "Aucun travail de sauvegarde.",
            ["JobStatus"] = "Statut: ",
            ["SourceMissing"] = "Le dossier source '{0}' n'existe pas. Sauvegarde ignorée.",
            ["TargetMissing"] = "Le dossier cible '{0}' n'existe pas. Sauvegarde ignorée.",
            ["CopyError"] = "Erreur lors de la copie de '{0}' : {1}",
            ["FullType"] = "Complète",
            ["DiffType"] = "Différentielle",
            ["ActiveStatus"] = "Active",
            ["CompletedStatus"] = "Terminée",
            ["RunAll"] = "Exécuter toutes les sauvegardes"
        };

        public static Dictionary<string, string> En = new Dictionary<string, string>
        {
            ["Title"] = "=== EasySave v1.0 ===",
            ["Menu"] = "\n1. Add a job\n2. List jobs\n3. Run a backup\n4. Run all backups\n0. Quit",
            ["Choice"] = "Your choice: ",
            ["NoSelection"] = "No selection entered.",
            ["InvalidChoice"] = "Invalid choice.",
            ["EnterIndices"] = "Enter indices (e.g., 1-3 or 1;3): ",
            ["BackupCompleted"] = "Backup(s) completed.",
            ["AddJob"] = "Job name: ",
            ["SourceDir"] = "Source directory: ",
            ["TargetDir"] = "Target directory: ",
            ["TypePrompt"] = "Type (1=Full, 2=Differential): ",
            ["AllFieldsRequired"] = "Error: all fields are required.",
            ["TypeInvalid"] = "Invalid type.",
            ["MaxJobs"] = "Maximum number of jobs reached.",
            ["JobAdded"] = "Job added.",
            ["NoJobs"] = "No backup job.",
            ["JobStatus"] = "Status: ",
            ["SourceMissing"] = "Source folder '{0}' does not exist. Backup skipped.",
            ["TargetMissing"] = "Target folder '{0}' does not exist. Backup skipped.",
            ["CopyError"] = "Error copying '{0}': {1}",
            ["FullType"] = "Full",
            ["DiffType"] = "Differential",
            ["ActiveStatus"] = "Active",
            ["CompletedStatus"] = "Completed",
            ["RunAll"] = "Run all backups"
        };
    }
}
