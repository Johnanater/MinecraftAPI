using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MinecraftAPI.Controllers
{
    [Route("api/Minecraft/[controller]")]
    [ApiController]
    public class GetSkinController : ControllerBase
    {
        // api/Minecraft/GetSkin/?uuid={uuid}
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            if (string.IsNullOrEmpty(HttpContext.Request.Query["uuid"]))
                return null;
            
            string uuid = HttpContext.Request.Query["uuid"];

            await Program.Utils.RetrieveSkin(uuid);

            var dir = "skincache/";
            //var path = Path.Combine(dir, uuid + ".png");
            var image = System.IO.File.OpenRead(dir + uuid + ".png");
            //if (!System.IO.File.Exists(path)) return null;

            return File(image, "image/png");
        }
    }
}