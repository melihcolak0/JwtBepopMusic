using Humanizer.Localisation;
using Jwt.PresentationLayer.Dtos.SongDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace Jwt.PresentationLayer.Controllers
{
    [Authorize]
    public class DefaultController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DefaultController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);

                string userId = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                string username = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                string role = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                ViewBag.UserId = userId;
                ViewBag.UserName = username;
                ViewBag.UserRole = role;
            }
            return View();
        }      
    }
}
