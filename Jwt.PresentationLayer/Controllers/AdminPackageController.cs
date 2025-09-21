using Jwt.PresentationLayer.Dtos.PackageDtos;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace Jwt.PresentationLayer.Controllers
{
    public class AdminPackageController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminPackageController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7157/api/Package");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Paketler alınamadı!";
                return View(new List<ResultPackageDto>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var values = JsonSerializer.Deserialize<List<ResultPackageDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(values);
        }

        public async Task<IActionResult> PackageDetails(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7157/api/Package/{id}");

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var package = JsonSerializer.Deserialize<ResultPackageDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(package);
        }

        [HttpGet]
        public IActionResult CreatePackage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePackage(CreatePackageDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonSerializer.Serialize(dto);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7157/api/Package", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError("", "Paket eklenemedi!");
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> UpdatePackage(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7157/api/Package/{id}");

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var package = JsonSerializer.Deserialize<UpdatePackageDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(package);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePackage(UpdatePackageDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonSerializer.Serialize(dto);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PutAsync("https://localhost:7157/api/Package", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError("", "Paket güncellenemedi!");
            return View(dto);
        }

        public async Task<IActionResult> DeletePackage(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"https://localhost:7157/api/Package/{id}");

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return BadRequest("Paket silinemedi!");
        }
    }
}
