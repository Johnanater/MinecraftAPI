using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MinecraftAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GetSkinController : ControllerBase
    {
        private readonly Utils _utils;
        
        public GetSkinController(Utils utils)
        {
            _utils = utils;
        }
        
        // /GetSkin/?uuid={uuid}
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            if (string.IsNullOrEmpty(HttpContext.Request.Query["uuid"]))
                return new BadRequestResult();
            
            string uuid = HttpContext.Request.Query["uuid"];
            
            var playerData = await _utils.GetPlayerData(uuid);

            if (playerData?.Skin == null)
                return new NotFoundResult();
            
            var image = Convert.FromBase64String(playerData.Skin);

            return File(image, "image/png");
        }
    }
}
