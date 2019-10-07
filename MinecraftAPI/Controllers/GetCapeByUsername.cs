using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MinecraftAPI.Controllers
{
    [Route("api/Minecraft/[controller]")]
    [ApiController]
    public class GetCapeByUsername : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            if (string.IsNullOrEmpty(HttpContext.Request.Query["username"])) return null;
            string username = HttpContext.Request.Query["username"];

            if (username.Contains(".png"))
            {
                username = username.Replace(".png", "");
            }

            string uuid = await Program.Utils.RetrieveUUID(username);

            await Program.Utils.RetrieveCape(uuid);

            var dir = "capecache/";
            var path = dir + uuid + ".png";
            var image =  System.IO.File.OpenRead(path);
            //if (!System.IO.File.Exists(path)) return null;

            return File(image, "image/png");
        }
    }
}