using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MinecraftAPI.Controllers
{
    
    [Route("api/Minecraft/[controller]")]
    [ApiController]
    public class GetSkinByUsernameController : ControllerBase
    {
        // api/Minecraft/GetSkinByUsername/?username={username}
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

            var playerData = await Program.Utils.GetPlayerDataFromUsername(username);

            if (playerData?.Skin == null)
                return new EmptyResult();
            
            var image = Convert.FromBase64String(playerData.Skin);

            return File(image, "image/png");
        }
    }
}
