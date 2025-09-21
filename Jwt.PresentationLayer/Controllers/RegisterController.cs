using Jwt.PresentationLayer.Dtos;
using Jwt.PresentationLayer.Dtos.PackageDtos;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace Jwt.PresentationLayer.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RegisterController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
                        
            var response = await client.GetAsync("https://localhost:7157/api/Package");
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var packages = JsonSerializer.Deserialize<List<ResultPackageDto>>(jsonData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                ViewBag.Packages = packages;
            }

            return View(new RegisterUserDto());
        }

        [HttpPost]
        public async Task<IActionResult> Index(RegisterUserDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient();

            var jsonData = JsonSerializer.Serialize(model);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7157/api/Auth/register", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Kayıt başarılı!";
                return RedirectToAction("Index", "Login");
            }

            ModelState.AddModelError("", "Kayıt başarısız!");
            return View(model);
        }
    }
}
