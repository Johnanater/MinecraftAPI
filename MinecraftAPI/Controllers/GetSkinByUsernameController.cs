using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MinecraftAPI.Controllers
{
    
    [Route("[controller]")]
    [ApiController]
    public class GetSkinByUsernameController : ControllerBase
    {
        private readonly Utils _utils;
        
        public GetSkinByUsernameController(Utils utils)
        {
            _utils = utils;
        }
        
        // /GetSkinByUsername/?username={username}
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

            var playerData = await _utils.GetPlayerDataFromUsername(username);

            // Player has no skin, return empty
            if (playerData?.Skin == null)
                return new EmptyResult();

            var image = Convert.FromBase64String(playerData.Skin);

            return File(image, "image/png");
        }
    }
}
