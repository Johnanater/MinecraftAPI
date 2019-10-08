using Newtonsoft.Json;

namespace MinecraftAPI.Models
{
    public class SessionProfileProperties
    {
        [JsonProperty("timestamp")]
        public long Timestamp;

        [JsonProperty("profileId")]
        public string ProfileId;

        [JsonProperty("profileName")]
        public string ProfileName;

        [JsonProperty("textures")]
        public Textures Textures;
    }
    
    public class Skin
    {
        [JsonProperty("url")]
        public string Url;
    }
    
    public class Cape
    {
        [JsonProperty("url")]
        public string Url;
    }

    public class Textures
    {
        [JsonProperty("SKIN")]
        public Skin Skin;

        [JsonProperty("CAPE")]
        public Cape Cape;
    }
}
