using Bring2mind.Backup.Restore.Common;
using Bring2mind.Backup.Restore.FileSystem;
using Bring2mind.Backup.Restore.Services;
using System;
using System.IO;
using System.Linq;

namespace Bring2mind.Backup.Restore
{
    class Program
    {
        private const string ConfigFile = @".\restore.config";
        private static StreamWriter Report { get; set; }

        static void Main(string[] args)
        {
            if (!File.Exists(ConfigFile))
            {
                var c = new Configuration();
                Globals.SaveObject(ConfigFile, c);
                return;
            }
            var config = Globals.GetObject<Configuration>(ConfigFile, null);
            Report = new StreamWriter($@"D:\{DateTime.Now.ToString("yyyy-MM-dd-hh-mm")}.log");

            var lastList = new DirectoryInfo(Path.Combine(config.BackupFolder, "Lists")).GetFiles("*.zip").LastOrDefault();
            if (lastList != null)
            {
                var list = Globals.GetZippedObject<FileSystemReport>(lastList.FullName);
                RestoreFolder(config, list.Root, config.RestoreFolder);
            }

            Report.WriteLine();
            Report.Close();
            Report.Dispose();

            Console.WriteLine("Done");
            Console.ReadLine();
        }

        private static void RestoreFolder(Configuration config, DiskFolder folder, string restoreFolder)
        {
            var saveFolder = folder.Path != "" ? Path.Combine(restoreFolder, folder.Path) : restoreFolder;
            saveFolder.EnsureExists();
            foreach (var f in folder.Files)
            {
                var hash = f.Hash;
                if (!string.IsNullOrEmpty(f.Hash))
                {
                    var origin = @$"{config.BackupFolder}\Files\{hash.Substring(0, 2)}\{hash.Substring(0, 5)}\{hash}";
                    if (File.Exists(origin))
                    {
                        CryptoManager.DecryptFile(origin, Path.Combine(saveFolder, f.Name), config.DecryptionKey, config.HostGuid);
                    }
                    else
                    {
                        Console.WriteLine($"Couldn't find {f.Name} to put in {saveFolder}");
                        Report.WriteLine($"Couldn't find {f.Name} to put in {saveFolder}");
                        Console.WriteLine(f.Hash);
                    }
                }
            }
            foreach (var f in folder.Folders)
            {
                RestoreFolder(config, f, restoreFolder);
            }
        }
    }
}
