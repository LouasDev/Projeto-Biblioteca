// Utils/AppPaths.cs
using System;
using System.IO;

namespace BibliotecaApp.Utils
{
    public static class AppPaths
    {
        public static string AppDataFolder =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BibliotecaApp");

        public static string BackupCacheFolder => Path.Combine(AppDataFolder, "BackupsPendentes");
        public static string MappingFolder => Path.Combine(AppDataFolder, "mapeamentoanual");
        public static string ReportsFolder => Path.Combine(AppDataFolder, "Relatorios");
        public static string GoogleTokenFolder => Path.Combine(AppDataFolder, "google-token");

        public static string RegistroBackupFile => Path.Combine(AppDataFolder, "ultimo_backup.txt");

        public static void EnsureFolders()
        {
            try
            {
                Directory.CreateDirectory(AppDataFolder);
                Directory.CreateDirectory(BackupCacheFolder);
                Directory.CreateDirectory(MappingFolder);
                Directory.CreateDirectory(ReportsFolder);
                Directory.CreateDirectory(GoogleTokenFolder);
            }
            catch
            {
                // Não levantar aqui — quem chamar pode tratar. Mas tentamos garantir diretórios.
            }
        }
    }
}
