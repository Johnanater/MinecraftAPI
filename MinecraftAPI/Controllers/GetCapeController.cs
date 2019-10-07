using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MinecraftAPI.Controllers
{
    [Route("api/Minecraft/[controller]")]
    [ApiController]
    public class GetCapeController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            if (string.IsNullOrEmpty(HttpContext.Request.Query["uuid"])) return null;
            string uuid = HttpContext.Request.Query["uuid"];

            await Program.Utils.RetrieveCape(uuid);

            var dir = "capecache/";
            var image = System.IO.File.OpenRead(dir + uuid + ".png");

            //if (!System.IO.File.Exists(image)) return null;

            return File(image, "image/png");
        }
    }
}