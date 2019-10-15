using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MinecraftAPI.Controllers
{
    [Route("api/Minecraft/[controller]")]
    [ApiController]
    public class GetCapeByUsername : ControllerBase
    {
        // api/Minecraft/GetCapeByUsername/?username={username}
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

            // Wait a second, due to race conditions and Mojang
            await Task.Delay(1000);
            
            var playerData = await Program.Utils.GetPlayerDataFromUsername(username);

            if (playerData?.Cape == null)
                return new EmptyResult();
            
            var image = Convert.FromBase64String(playerData.Cape);

            return File(image, "image/png");
        }
    }
}
