using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MinecraftAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GetCapeByUsername : ControllerBase
    {
        private readonly Utils _utils;
        
        public GetCapeByUsername(Utils utils)
        {
            _utils = utils;
        }
        
        // /GetCapeByUsername/?username={username}
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            if (string.IsNullOrEmpty(HttpContext.Request.Query["username"]))
                return null;
            
            string username = HttpContext.Request.Query["username"];

            // Remove .png, for Minecraft clients
            if (username.Contains(".png"))
            {
                username = username.Replace(".png", "");
            }

            // Wait 5 seconds, due to race conditions and Mojang
            await Task.Delay(5000);
            
            var playerData = await _utils.GetPlayerDataFromUsername(username);

            if (playerData?.Cape == null)
                return new EmptyResult();
            
            var image = Convert.FromBase64String(playerData.Cape);

            return File(image, "image/png");
        }
    }
}
