using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MinecraftAPI.Models;
using Newtonsoft.Json;

namespace MinecraftAPI
{
    public class JsonUtils
    {
        private readonly SemaphoreSlim _writeLock = new SemaphoreSlim(1, 1);
        
        public string CacheFile = "./Cache/cache.json";

        // Check if there is a player with the same UUID in the cache
        public async Task<bool> InCache(string uuid)
        {
            var cache = await ReadFromCache();

            var player = cache.Players.FirstOrDefault(obj => obj.Uuid.Equals(uuid));

            // If the cache is older than time in config, renew it
            return player?.LastUpdated.AddSeconds(Program.Instance.CacheTimeSeconds) > DateTime.Now;
        }
        
        // Check if there is a player with the same username in the cache
        public async Task<bool> UsernameInCache(string username)
        {
            var cache = await ReadFromCache();

            var player = cache.Players.FirstOrDefault(obj => obj.Username.ToLower().Equals(username.ToLower()));
            
            // If the cache is older than time in config, renew it
            return player?.LastUpdated.AddSeconds(Program.Instance.CacheTimeSeconds) > DateTime.Now;
        }
        
        // Get player data from UUID
        public async Task<PlayerData> GetPlayerData(string uuid)
        {
            var cache = await ReadFromCache();
            
            // Find in cache
            var playerData = cache.Players.FirstOrDefault(obj => obj.Uuid.Equals(uuid));

            return playerData;
        }
        
        // Get player data from username
        public async Task<PlayerData> GetPlayerDataFromUsername(string username)
        {
            var cache = await ReadFromCache();
            
            // Find in cache
            var playerData = cache.Players.FirstOrDefault(obj => obj.Username.Equals(username));

            return playerData;
        }

        // Set player data in cache
        public async Task SetPlayerData(PlayerData playerData)
        {
            var cache = await ReadFromCache();
            
            // If already in cache, delete it
            if (cache.Players.Any(obj => obj.Uuid.Equals(playerData.Uuid)))
            {
                cache.Players.RemoveAll(obj => obj.Uuid.Equals(playerData.Uuid));
            }
            
            cache.Players.Add(playerData);

            await WriteToCache(cache);
        }
        
        // Convert to json
        public string ToJson(Cache cache)
        {
            string json;

            try
            {
                json = JsonConvert.SerializeObject(cache, Formatting.Indented);
            }
            catch (Exception e)
            {
                Console.Write("Failed to serialize JSON");
                Console.WriteLine(e);
                throw;
            }

            return json;
        }

        // Convert from json
        public Cache FromJson(string json)
        {
            Cache cache;
            try
            {
                cache = JsonConvert.DeserializeObject<Cache>(json);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return cache;
        }

        // Read from the cache
        public async Task<Cache> ReadFromCache()
        {
            Cache cache;
            
            try
            {
                await CheckCache();
                string json; 
                using (StreamReader reader = new StreamReader(CacheFile))
                {
                    json = await reader.ReadToEndAsync();
                }
                
                cache = FromJson(json);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to read from cache!");
                Console.WriteLine(e);
                throw;
            }

            return cache;
        }

        // Write to the cache
        public async Task WriteToCache(Cache cache)
        {
            await _writeLock.WaitAsync();
                        
            var json = ToJson(cache);

            try
            {
                await CheckCache();
                using (StreamWriter writer = new StreamWriter(CacheFile))
                {
                    await writer.WriteAsync(json);
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error: Failed to write to cache!");
                Console.WriteLine(ex);
                throw;
            }
            finally
            {
                _writeLock.Release();
            }
        }

        // Check the cache, create if not exist
        public async Task CheckCache()
        {
            if (!File.Exists(CacheFile))
            {
                Console.WriteLine("Cache not found, creating...");
                await CreateCache();
            }
        }

        // Create the cache
        public async Task CreateCache()
        {
            await _writeLock.WaitAsync();

            var cache = new Cache
            {
                Players = new List<PlayerData>()
            };
            
            try
            {
                Directory.CreateDirectory(CacheFile.Replace("cache.json", ""));
                using (StreamWriter writer = new StreamWriter(CacheFile))
                {
                    await writer.WriteAsync(ToJson(cache));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: Failed to create cache!");
                Console.WriteLine(ex);
                throw;
            }
            finally
            {
                _writeLock.Release();
            }
        }
    }
}
