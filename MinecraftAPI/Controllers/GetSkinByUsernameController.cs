using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;

namespace MinecraftAPI.Controllers
{
    
    [Route("api/Minecraft/[controller]")]
    [ApiController]
    public class GetSkinByUsernameController : ControllerBase
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

            await Program.Utils.RetrieveSkin(uuid);

            var dir = "skincache/";
            var image = System.IO.File.OpenRead(dir + uuid + ".png");
            //if (!System.IO.File.Exists(image)) return null;

            return File(image, "image/png");
        }
    }
}