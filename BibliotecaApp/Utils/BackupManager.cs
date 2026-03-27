// BackupManager.cs
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace BibliotecaApp.Services
{
    public static class RegistroBackup
    {
        public static DateTime? LerUltimoBackup(string path)
        {
            try
            {
                if (!File.Exists(path)) return null;
                var txt = File.ReadAllText(path).Trim();
                if (DateTime.TryParse(txt, out var dt)) return dt;
                return null;
            }
            catch { return null; }
        }

        public static bool JaFezHoje(string path)
        {
            var dt = LerUltimoBackup(path);
            return dt.HasValue && dt.Value.Date == DateTime.Now.Date;
        }

        public static void GravarHoje(string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllText(path, DateTime.Now.ToString("yyyy-MM-dd"));
        }
    }

    public static class BackupLocal
    {
        public static string CriarZipBanco(string sdfPath, string destinoDir)
        {
            Directory.CreateDirectory(destinoDir);

            if (!File.Exists(sdfPath))
                throw new FileNotFoundException("Arquivo do banco não encontrado.", sdfPath);

            string zipPath = Path.Combine(destinoDir, $"backup_{DateTime.Now:yyyy-MM-dd_HH-mm}.zip");

            // Cria zip usando ZipArchive (compatível com C# 7.3)
            using (FileStream zipToOpen = new FileStream(zipPath, FileMode.Create))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create))
                {
                    // CreateEntryFromFile precisa do assembly System.IO.Compression.FileSystem em .NET Framework
                    archive.CreateEntryFromFile(sdfPath, Path.GetFileName(sdfPath), CompressionLevel.Optimal);
                }
            }

            return zipPath;
        }
    }

    public static class DriveHelper
    {
        private const string NomePasta = "Backups Biblioteca";

        public static DriveService CriarServico(string credentialsJsonPath, string tokenDir)
        {
            if (!File.Exists(credentialsJsonPath))
                throw new FileNotFoundException("credentials.json não encontrado", credentialsJsonPath);

            using (var stream = new FileStream(credentialsJsonPath, FileMode.Open, FileAccess.Read))
            {
                var secrets = GoogleClientSecrets.FromStream(stream).Secrets;

                var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    secrets,
                    new[] { DriveService.Scope.DriveFile, DriveService.Scope.Drive },
                    "user",
                    CancellationToken.None,
                    new FileDataStore(tokenDir, true)).Result;

                var service = new DriveService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "BibliotecaApp"
                });

                return service;
            }
        }

        public static string ObterOuCriarPasta(DriveService svc)
        {
            var list = svc.Files.List();
            list.Q = $"name = '{NomePasta}' and mimeType = 'application/vnd.google-apps.folder' and trashed = false";
            list.Fields = "files(id, name)";
            var res = list.Execute();

            if (res.Files != null && res.Files.Count > 0)
                return res.Files[0].Id;

            var folderMetadata = new Google.Apis.Drive.v3.Data.File
            {
                Name = NomePasta,
                MimeType = "application/vnd.google-apps.folder"
            };
            var create = svc.Files.Create(folderMetadata);
            create.Fields = "id";
            var folder = create.Execute();
            return folder.Id;
        }

        public static string UploadArquivo(DriveService svc, string folderId, string localPath)
        {
            var fileMeta = new Google.Apis.Drive.v3.Data.File
            {
                Name = Path.GetFileName(localPath),
                Parents = new List<string> { folderId }
            };

            using (var fs = new FileStream(localPath, FileMode.Open, FileAccess.Read))
            {
                var req = svc.Files.Create(fileMeta, fs, "application/zip");
                req.Fields = "id, name, createdTime, size";
                req.Upload();
                return req.ResponseBody?.Id;
            }
        }

        public static void ExcluirAntigos(DriveService svc, string folderId, int dias = 30, int manterMinimo = 10)
        {
            var list = svc.Files.List();
            list.Q = $"'{folderId}' in parents and mimeType != 'application/vnd.google-apps.folder' and trashed = false";
            list.Fields = "files(id, name, createdTime)";
            list.PageSize = 1000;
            var res = list.Execute();
            if (res.Files == null || res.Files.Count == 0) return;

            var ordenados = res.Files.OrderByDescending(f => f.CreatedTimeDateTimeOffset).ToList();
            var limiteData = DateTime.UtcNow.AddDays(-dias);

            var candidatos = ordenados
                .Select((f, idx) => new { f, idx })
                .Where(x => x.f.CreatedTimeDateTimeOffset < limiteData && x.idx >= manterMinimo)
                .Select(x => x.f)
                .ToList();

            foreach (var f in candidatos)
            {
                try { svc.Files.Delete(f.Id).Execute(); }
                catch { /* opcional: log */ }
            }
        }
    }

    public static class BackupDiario
    {
        public static void Executar(string caminhoSdf, string registroBackupPath, string appDataDir, string backuplocaisDir, string credentialsJsonPath)
        {
            try
            {
                if (RegistroBackup.JaFezHoje(registroBackupPath))
                    return;

                string zipPath = BackupLocal.CriarZipBanco(caminhoSdf, backuplocaisDir);

                var tokenDir = Path.Combine(appDataDir, "google-token");
                var svc = DriveHelper.CriarServico(credentialsJsonPath, tokenDir);
                var folderId = DriveHelper.ObterOuCriarPasta(svc);
                DriveHelper.UploadArquivo(svc, folderId, zipPath);

                RegistroBackup.GravarHoje(registroBackupPath);

                DriveHelper.ExcluirAntigos(svc, folderId, dias: 30, manterMinimo: 10);

                try { File.Delete(zipPath); } catch { /* manter se preferir */ }
            }
            catch (Exception ex)
            {
                try
                {
                    var logPath = Path.Combine(appDataDir, "backup_errors.log");
                    File.AppendAllText(logPath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {ex}\r\n");
                }
                catch { }
            }
        }

        public static void ReenviarPendentes(string appDataDir, string backuplocaisDir, string credentialsJsonPath)
        {
            try
            {
                var tokenDir = Path.Combine(appDataDir, "google-token");
                var svc = DriveHelper.CriarServico(credentialsJsonPath, tokenDir);
                var folderId = DriveHelper.ObterOuCriarPasta(svc);

                var files = Directory.GetFiles(backuplocaisDir, "*.zip").OrderBy(f => f).ToList();
                foreach (var f in files)
                {
                    try
                    {
                        DriveHelper.UploadArquivo(svc, folderId, f);
                        File.Delete(f);
                    }
                    catch
                    {
                        // se falhar, deixar para próxima tentativa
                    }
                }

                DriveHelper.ExcluirAntigos(svc, folderId, dias: 30, manterMinimo: 10);
            }
            catch { /* log se quiser */ }
        }
    }
}
