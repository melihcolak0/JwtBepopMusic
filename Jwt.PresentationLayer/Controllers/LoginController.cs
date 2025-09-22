using Jwt.PresentationLayer.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Jwt.PresentationLayer.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }        

        [HttpPost]
        public async Task<IActionResult> Index(LoginUserDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient();

            var jsonData = JsonSerializer.Serialize(model);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7157/api/Auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var json = JsonSerializer.Deserialize<JsonElement>(responseData);
                var token = json.GetProperty("token").GetString();

                // Session'a yaz
                HttpContext.Session.SetString("JWToken", token);

                // JWT içinden kullanıcı bilgilerini çöz (örnek: kullanıcı adı, rol)
                var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);

                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? ""),
            new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? ""),
            new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "User")
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Cookie auth için login ol
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties
                    {
                        IsPersistent = true, // tarayıcı kapanınca da kalsın mı?
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                    });

                return RedirectToAction("Index", "Default");
            }

            ModelState.AddModelError("", "Email veya şifre hatalı!");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("JWToken");

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index");
        }
    }
}
