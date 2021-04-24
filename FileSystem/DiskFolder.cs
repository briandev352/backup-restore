namespace Bring2mind.Backup.Restore.FileSystem
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    [JsonObject]
    public class DiskFolder
    {
        [JsonProperty("p")]
        public string Path { get; set; } = "";

        [JsonProperty("f")]
        public List<DiskFile> Files { get; set; } = new List<DiskFile>();

        [JsonProperty("d")]
        public List<DiskFolder> Folders { get; set; } = new List<DiskFolder>();

        [JsonProperty("nrf")]
        public int NrFiles { get; set; } = 0;

        [JsonProperty("nrd")]
        public int NrFolders { get; set; } = 0;
    }
}
