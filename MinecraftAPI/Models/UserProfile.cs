using Newtonsoft.Json;

namespace MinecraftAPI.Models
{
    public class UserProfile
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("name")]

        public string Name;
    }
}