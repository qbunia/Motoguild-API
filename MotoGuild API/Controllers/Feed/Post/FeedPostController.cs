using Microsoft.AspNetCore.Mvc;

namespace MotoGuild_API.Controllers
{
    public class FeedPostController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
