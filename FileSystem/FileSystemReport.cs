namespace Bring2mind.Backup.Restore.FileSystem
{
    using Newtonsoft.Json;

    [JsonObject]
    public class FileSystemReport
    {
        [JsonProperty("runtime")]
        public double RunSeconds { get; set; }

        [JsonProperty("created")]
        public string Created { get; set; }

        [JsonProperty("root")]
        public DiskFolder Root { get; set; }
    }
}
