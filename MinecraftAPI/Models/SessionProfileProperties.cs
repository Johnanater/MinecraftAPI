using Newtonsoft.Json;

namespace MinecraftAPI.Models
{
    public class SessionProfileProperties
    {
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("profileId")]
        public string ProfileId { get; set; }

        [JsonProperty("profileName")]
        public string ProfileName { get; set; }

        [JsonProperty("textures")]
        public Textures Textures { get; set; }
    }
    
    public class SKIN
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }
    
    public class CAPE
    {

        [JsonProperty("url")]
        public string Url;
    }

    public class Textures
    {
        [JsonProperty("SKIN")]
        public SKIN SKIN { get; set; }

        [JsonProperty("CAPE")]
        public CAPE CAPE;
    }
}