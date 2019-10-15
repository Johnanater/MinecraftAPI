using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MinecraftAPI.Controllers
{
    [Route("api/Minecraft/[controller]")]
    [ApiController]
    public class GetCapeController : ControllerBase
    {
        private readonly Utils _utils;
        
        public GetCapeController(Utils utils)
        {
            _utils = utils;
        }
        
        // api/Minecraft/GetCape/?uuid={uuid}
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            if (string.IsNullOrEmpty(HttpContext.Request.Query["uuid"]))
                return null;
            
            string uuid = HttpContext.Request.Query["uuid"];
            
            var playerData = await _utils.GetPlayerData(uuid);

            if (playerData?.Cape == null)
                return new EmptyResult();
            
            var image = Convert.FromBase64String(playerData.Cape);

            return File(image, "image/png");
        }
    }
}
