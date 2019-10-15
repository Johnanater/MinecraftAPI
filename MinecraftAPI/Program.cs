using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace MinecraftAPI
{
    public class Program
    {
        public static Program Instance;
        public static JsonUtils JsonUtils;

        public const string Version = "1.0.0.0";

        public int CacheTimeSeconds = 1800; // By default will cache for 1800 seconds, or 30 minutes
        
        public static void Main(string[] args)
        {
            Instance = new Program();
            JsonUtils = new JsonUtils();
            
            Console.WriteLine($"MinecraftAPI started, version {Version}");

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}
