using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            // Add Polly
            services.AddHttpClient("Minecraft") 
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))  
                // important step  
                .AddPolicyHandler(GetRetryPolicy());  
        }
        
        private IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()  
        {  
            return HttpPolicyExtensions  
                    // HttpRequestException, 5XX and 408  
                    .HandleTransientHttpError()  
                    // 404  
                    .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)  
                    // Retry two times after delay  
                    .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))  
                ;  
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
