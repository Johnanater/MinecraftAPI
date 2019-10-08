using System.Collections.Generic;
using Newtonsoft.Json;

namespace MinecraftAPI.Models
{
    public class SessionProfile
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("properties")]
        public List<SessionProfilePropertiesEncoded> Properties;

        [JsonProperty("legacy")]
        public bool Legacy;
    }
}
