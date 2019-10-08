using System.Collections.Generic;
using Newtonsoft.Json;

namespace MinecraftAPI.Models
{
    public class SessionProfilePropertiesEncoded
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("value")]
        public string Value;
    }
}
