using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MinecraftAPI.Models;

namespace MinecraftAPI
{
    public class Program
    {
        public static Utils Utils;
        public static JsonUtils JsonUtils;

        public const string Version = "1.0.0.0";
        
        public static void Main(string[] args)
        {
            Utils = new Utils();
            JsonUtils = new JsonUtils();
            
            Console.WriteLine($"MinecraftAPI started, version {Version}");
            
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}