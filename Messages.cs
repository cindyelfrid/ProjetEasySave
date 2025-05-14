namespace ProjetEasySave
{
    public static class Messages
    {
        public static Dictionary<string, string> Fr = new Dictionary<string, string>
        {
            ["Title"] = "=== EasySave v1.0 ===",
            ["Menu"] = "\n1. Ajouter un travail\n2. Lister les travaux\n3. Exécuter la sauvegarde\n0. Quitter",
            ["Choice"] = "Votre choix : ",
            ["NoSelection"] = "Aucune sélection saisie.",
            ["InvalidChoice"] = "Choix invalide.",
            ["EnterIndices"] = "Entrez les indices (ex : 1-3 ou 1;3) : ",
            ["BackupCompleted"] = "Sauvegarde(s) terminée(s)."
        };

        public static Dictionary<string, string> En = new Dictionary<string, string>
        {
            ["Title"] = "=== EasySave v1.0 ===",
            ["Menu"] = "\n1. Add a job\n2. List jobs\n3. Execute backup\n0. Quit",
            ["Choice"] = "Your choice: ",
            ["NoSelection"] = "No selection entered.",
            ["InvalidChoice"] = "Invalid choice.",
            ["EnterIndices"] = "Enter indices (e.g., 1-3 or 1;3): ",
            ["BackupCompleted"] = "Backup(s) completed."
        };
    }
}
