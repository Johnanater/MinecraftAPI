using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MinecraftAPI.Models;
using Newtonsoft.Json;

namespace MinecraftAPI
{
    public class Utils
    {
        private const string GetProfileUrl = "https://api.mojang.com/users/profiles/minecraft/{0}";

        // Retrieve UUID from Mojang
        public async Task<string> RetrieveUUID(string username)
        {
            using (WebClient webClient = new WebClient())
            {
                var jsonDownload = await webClient.DownloadStringTaskAsync(string.Format(GetProfileUrl, username));

                try
                {
                    var userProfile = JsonConvert.DeserializeObject<UserProfile>(jsonDownload);
                    return userProfile.Id;
                }
                catch
                {
                    return null;
                }
            }
        }
        
        // Retrieve skin from Mojang
        // TODO: Make actually use async
        public async Task RetrieveSkin(string uuid)
        {
            string skinFile = "skincache/" + uuid + ".png";
            string skinCache = "skincache";

            // If the file is younger than an hour, do not fetch again
            if (File.Exists(skinFile) && File.GetLastWriteTime(skinFile).AddHours(1) > DateTime.Now)
            {
                return;
            }
            try
            {
                var sessionProfile = GetSessionProfile(uuid);

                SessionProfileProperties jsonProfileProperties = DecodeProperties(sessionProfile.Properties.FirstOrDefault());

                using (WebClient webClient = new WebClient())
                {
                    if (!Directory.Exists(skinCache))
                    {
                        Directory.CreateDirectory(skinCache);
                    }
                    webClient.DownloadFile(jsonProfileProperties.Textures.SKIN.Url, skinFile);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("RetrieveSkin error: " + uuid);
                Console.WriteLine("Error message: " + e.Message + Environment.NewLine);
                Console.WriteLine(e);
            }
        }
        
        // Retrieve Cape from Mojang
        // TODO: Make actually async
        public async Task RetrieveCape(string uuid)
        {
            string capeFile = "capecache/" + uuid + ".png";
            string capeCache = "capecache/";

            if (System.IO.File.Exists(capeFile) && System.IO.File.GetLastWriteTime(capeFile).AddHours(1) > DateTime.Now)
            {
                return;
            }

            try
            {
                var sessionProfile = GetSessionProfile(uuid);

                var jsonProperties = DecodeProperties(sessionProfile.Properties.FirstOrDefault());

                if (jsonProperties.Textures.CAPE == null) return;

                using (WebClient webClient = new WebClient())
                {
                    if (!Directory.Exists(capeCache))
                    {
                        Directory.CreateDirectory(capeCache);
                    }
                    webClient.DownloadFile(jsonProperties.Textures.CAPE.Url, capeFile);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("RetrieveCape error: " + uuid);
                Console.WriteLine("Error message: " + e.Message + Environment.NewLine);
                Console.WriteLine(e);
            }
        }
        
        public SessionProfile GetSessionProfile(string uuid)
        {
            using (WebClient webClient = new WebClient())
            {
                var jsonDownload = webClient.DownloadString(string.Format("https://sessionserver.mojang.com/session/minecraft/profile/{0}", uuid));

                var json = JsonConvert.DeserializeObject<SessionProfile>(jsonDownload);

                return json;
            }
        }
        
        public SessionProfileProperties DecodeProperties(SessionProfilePropertiesEncoded properties)
        {
            byte[] decoded = Convert.FromBase64String(properties.Value);
            string decodedString = System.Text.Encoding.UTF8.GetString(decoded);

            var json = JsonConvert.DeserializeObject<SessionProfileProperties>(decodedString);

            return json;
        }
    }
}