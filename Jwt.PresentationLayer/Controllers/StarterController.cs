using Microsoft.AspNetCore.Mvc;

namespace Jwt.PresentationLayer.Controllers
{
    public class StarterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
