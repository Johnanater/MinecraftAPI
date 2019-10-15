using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace MinecraftAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // TODO: Implement a retry on errors
        // TODO: The current Polly implementation not work because the Dependency Injection does not work with external classes or something
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            // Add Polly
            services
                .AddHttpClient<Utils>()
                .AddTransientHttpErrorPolicy(
                    x => x.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(3, retryAttempt))));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (int.TryParse(Configuration["cacheTime"], out int cacheTime))
                Program.Instance.CacheTimeSeconds = cacheTime;

            if (env.IsDevelopment())
            {
                Program.JsonUtils.CacheFile = $"../Cache/cache.json";
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts(); // REMOVE HTTPS
            }

            // Remove HTTPS
            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
