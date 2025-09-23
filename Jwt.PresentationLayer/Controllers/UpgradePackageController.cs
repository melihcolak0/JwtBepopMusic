using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;

namespace Jwt.PresentationLayer.Controllers
{
    [Authorize]
    public class UpgradePackageController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UpgradePackageController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            string userId = null;
            //Jwt Token
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);

                userId = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                string username = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                string role = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                ViewBag.UserId = userId;
                ViewBag.UserName = username;
                ViewBag.UserRole = role;
            }

            // UserPackageId Consume
            int userPackageId = 1; // default (en düşük paket)

            if (!string.IsNullOrEmpty(userId))
            {
                var client2 = _httpClientFactory.CreateClient();
                var userResponse = await client2.GetAsync($"https://localhost:7157/api/User/GetUserPackageIdByUserId/{userId}");

                if (userResponse.IsSuccessStatusCode)
                {
                    var userJson = await userResponse.Content.ReadAsStringAsync();
                    // Diyelim ki API sadece int dönüyor
                    userPackageId = JsonSerializer.Deserialize<int>(userJson);
                }
            }
            ViewBag.UserPackageId = userPackageId;

            return View();
        }
    }
}
