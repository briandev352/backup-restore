namespace Bring2mind.Backup.Restore.Services
{
    using Bring2mind.Backup.Restore.Common;
    using System.IO;
    using System.IO.Compression;

    public class CompressionManager
    {
        public static string UnzipTextFile(string zipFile)
        {
            var fileContent = "";
            using (var zipIn = new ZipArchive(File.OpenRead(zipFile), ZipArchiveMode.Read))
            {
                var firstEntry = zipIn.Entries[0];
                using (var inStream = firstEntry.Open())
                {
                    fileContent = inStream.ToEncodedString();
                }
            }
            return fileContent;
        }
    }
}
