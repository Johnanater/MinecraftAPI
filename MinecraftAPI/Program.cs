using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace MinecraftAPI
{
    public class Program
    {
        public static Program Instance;
        public static Utils Utils;
        public static JsonUtils JsonUtils;

        public const string Version = "1.0.0.0";

        public int CacheTimeSeconds = 1800; // By default will cache for 1800 seconds, or 30 minutes
        
        public static void Main(string[] args)
        {
            Instance = new Program();
            Utils = new Utils();
            JsonUtils = new JsonUtils();
            
            Console.WriteLine($"MinecraftAPI started, version {Version}");

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddCommandLine(args);
                })
                .UseStartup<Startup>();
    }
}
