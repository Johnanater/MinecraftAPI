using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;

namespace MinecraftAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            
            services.AddMvc()
                .AddNewtonsoftJson();

            // Add Polly policy
            var policy = HttpPolicyExtensions
                .HandleTransientHttpError() // HttpRequestException, 5XX and 408
                .OrResult(response => (int)response.StatusCode == 429) // RetryAfter
                .WaitAndRetryAsync(new []
                {
                    TimeSpan.FromSeconds(30),
                    TimeSpan.FromSeconds(60),
                    TimeSpan.FromSeconds(90)
                });
            
            services
                .AddHttpClient<Utils>()
                .AddPolicyHandler(policy);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (int.TryParse(Configuration["cacheTime"], out int cacheTime))
                Program.Instance.CacheTimeSeconds = cacheTime;

            if (env.IsDevelopment())
            {
                Program.JsonUtils.CacheFile = $"../Cache/cache.json";
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
