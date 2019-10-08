using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MinecraftAPI.Controllers
{
    [Route("api/Minecraft/[controller]")]
    [ApiController]
    public class GetUUIDController : ControllerBase
    {
        // api/Minecraft/GetUUID/?username={username}
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            var username = HttpContext.Request.Query["username"];
            var uuid = await Program.Utils.RetrieveUUID(username);
            
            if (uuid == null)
                return new EmptyResult();
            
            return uuid; 
        }
    }
}
