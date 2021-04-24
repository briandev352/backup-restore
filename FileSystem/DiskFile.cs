namespace Bring2mind.Backup.Restore.FileSystem
{
    using Newtonsoft.Json;

    [JsonObject]
    public class DiskFile
    {
        [JsonProperty("n")]
        public string Name { get; set; } = "";
        [JsonProperty("h")]
        public string Hash { get; set; } = "";
    }
}
