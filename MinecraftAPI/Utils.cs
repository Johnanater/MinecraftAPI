using System;
using System.Net.Http;
using System.Threading.Tasks;
using MinecraftAPI.Models;
using Newtonsoft.Json;

namespace MinecraftAPI
{
    public class Utils
    {
        public static readonly HttpClient HttpClient = new HttpClient();

        private const string GetProfileUrl = "https://api.mojang.com/users/profiles/minecraft/{0}";
        private const string GetSessionProfileUrl = "https://sessionserver.mojang.com/session/minecraft/profile/{0}";

        // Gets player data, checks for cache first
        public async Task<PlayerData> GetPlayerData(string uuid)
        {
            // Check if it is in the cache
            if (await Program.JsonUtils.InCache(uuid))
            {
                return await Program.JsonUtils.GetPlayerData(uuid);
            }
            
            // Else, retrieve it all
            return await RetrieveFromMojang(uuid);
        }
        
        // Gets player data, checks for cache first
        public async Task<PlayerData> GetPlayerDataFromUsername(string username)
        {
            // Check if it is in the cache
            if (await Program.JsonUtils.UsernameInCache(username))
            {
                return await Program.JsonUtils.GetPlayerDataFromUsername(username);
            }
            
            // Else, Find the UUID
            var uuid = await RetrieveUUID(username);

            // And retrieve it all
            return await RetrieveFromMojang(uuid);
        }

        // Retrieve PlayerData from Mojang
        public async Task<PlayerData> RetrieveFromMojang(string uuid)
        {
            var sessionProfile = await GetSessionProfile(uuid);

            if (sessionProfile == null)
            {
                Console.WriteLine($"Error: SessionProfile for {uuid} is null!");
                return null;
            }
            
            var properties = DecodeProperties(sessionProfile.Properties[0]);

            var playerData = new PlayerData
            {
                Uuid = uuid,
                Username =  sessionProfile.Name,
                Skin = await ImageToBase64(properties.Textures.Skin?.Url),
                Cape = await ImageToBase64(properties.Textures.Cape?.Url),
                LastUpdated = DateTime.Now
            };

            // Store it in the cache
            Program.JsonUtils.SetPlayerData(playerData).FireAndForget();

            return playerData;
        }

        // Retrieve UUID from Mojang
        public async Task<string> RetrieveUUID(string username)
        {
            var jsonDownload = await HttpClient.GetStringAsync(string.Format(GetProfileUrl, username));
            var userProfile = JsonConvert.DeserializeObject<UserProfile>(jsonDownload);
            
            return userProfile?.Id;
        }

        public async Task<string> ImageToBase64(string url)
        {
            if (url == null)
                return null;
            
            var bytes = await HttpClient.GetByteArrayAsync(url);
            
            return Convert.ToBase64String(bytes);
        }
        
        // Retrieve username from Mojang
        public async Task<string> RetrieveUsername(string uuid)
        {
            var sessionProfile = await GetSessionProfile(uuid);

            return sessionProfile.Name;
        }

        // Get a user's session profile from UUID
        public async Task<SessionProfile> GetSessionProfile(string uuid)
        {
            var response = await HttpClient.GetAsync(string.Format(GetSessionProfileUrl, uuid));

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: Got {response.StatusCode} when retrieving SessionProfile for {uuid}!");
                return null;
            }

            var jsonDownload = await response.Content.ReadAsStringAsync();
            
            var json = JsonConvert.DeserializeObject<SessionProfile>(jsonDownload);

            return json;
        }
        
        // Decode the Base64-encoded properties found in the SessionProfile
        public SessionProfileProperties DecodeProperties(SessionProfilePropertiesEncoded properties)
        {
            byte[] decoded = Convert.FromBase64String(properties.Value);
            string decodedString = System.Text.Encoding.UTF8.GetString(decoded);

            var json = JsonConvert.DeserializeObject<SessionProfileProperties>(decodedString);

            return json;
        }
    }
    
    // Extensions
    public static class Extensions
    {
        // From https://stackoverflow.com/a/27852439
        public static async void FireAndForget(this Task task)
        {
            try
            {
                await task;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
