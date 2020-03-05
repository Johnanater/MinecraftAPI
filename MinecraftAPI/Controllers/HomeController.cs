using Microsoft.AspNetCore.Mvc;

namespace MinecraftAPI.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Get()
        {
            return View($"Views/Index.cshtml");
        }
    }
}