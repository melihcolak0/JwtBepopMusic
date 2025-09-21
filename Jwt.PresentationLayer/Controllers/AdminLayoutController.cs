using Microsoft.AspNetCore.Mvc;

namespace Jwt.PresentationLayer.Controllers
{
    public class AdminLayoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
