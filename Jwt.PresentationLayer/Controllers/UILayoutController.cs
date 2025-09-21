using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Jwt.PresentationLayer.Controllers
{
    public class UILayoutController : Controller
    {
        public IActionResult Index()
        {                     
            return View();
        }
    }
}
