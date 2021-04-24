namespace Bring2mind.Backup.Restore
{
    using Newtonsoft.Json;

    [JsonObject]
    public class Configuration
    {
        [JsonProperty("decryptionKey")]
        public string DecryptionKey { get; set; } = "";

        [JsonProperty("hostGuid")]
        public string HostGuid { get; set; } = "";

        [JsonProperty("backupFolder")]
        public string BackupFolder { get; set; } = "";

        [JsonProperty("restoreFolder")]
        public string RestoreFolder { get; set; } = "";


        [JsonIgnore]
        public bool Finished { get; set; } = false;

        [JsonIgnore]
        public bool LastRun { get; set; } = false;

        public bool IsValid()
        {
            return !(string.IsNullOrEmpty(this.DecryptionKey)
                || string.IsNullOrEmpty(this.HostGuid)
                || string.IsNullOrEmpty(this.BackupFolder)
                || string.IsNullOrEmpty(this.RestoreFolder)
                );
        }
    }
}
