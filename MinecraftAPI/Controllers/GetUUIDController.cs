using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MinecraftAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GetUUIDController : ControllerBase
    {
        private readonly Utils _utils;
        
        public GetUUIDController(Utils utils)
        {
            _utils = utils;
        }
        
        // /GetUUID/?username={username}
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            if (string.IsNullOrEmpty(HttpContext.Request.Query["username"]))
                return new BadRequestResult();
            
            var username = HttpContext.Request.Query["username"];
            var uuid = await _utils.RetrieveUUID(username);
            
            if (uuid == null)
                return new NotFoundResult();
            
            return uuid; 
        }
    }
}
