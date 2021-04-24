using System.IO;

namespace Bring2mind.Backup.Restore.Common
{
    public class Globals
    {
        public static T GetObject<T>(string filename, T defaultObject)
        {
            T res = defaultObject;
            if (File.Exists(filename))
            {
                using (var sr = new StreamReader(filename))
                {
                    var list = sr.ReadToEnd();
                    res = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(list);
                }
            }
            return res;
        }
        public static void SaveObject(string filename, object objectToSave)
        {
            using (var sw = new StreamWriter(filename))
            {
                sw.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(objectToSave, Newtonsoft.Json.Formatting.Indented));
            }
        }

        public static T GetZippedObject<T>(string filename)
        {
            var fileContent = "";
            fileContent = Services.CompressionManager.UnzipTextFile(filename);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(fileContent);
        }

        public static void CopyBigStream(Stream source, Stream destination, int bufferSize)
        {
            var bBuffer = new byte[bufferSize + 1];
            int iLengthOfReadChunk;
            do
            {
                iLengthOfReadChunk = source.Read(bBuffer, 0, bufferSize);
                destination.Write(bBuffer, 0, iLengthOfReadChunk);
                if (iLengthOfReadChunk == 0)
                    break;
            }
            while (true);
        }

        public static void CopyBigStream(Stream source, Stream destination)
        {
            CopyBigStream(source, destination, 25000);
        }
    }
}
