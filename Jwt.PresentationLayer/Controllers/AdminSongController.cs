using Jwt.PresentationLayer.Dtos.PackageDtos;
using Jwt.PresentationLayer.Dtos.SongDtos;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Jwt.PresentationLayer.Controllers
{
    public class AdminSongController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminSongController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7157/api/Song/GetListSongsWithPackages");

            if (!response.IsSuccessStatusCode)
                return View(new List<ResultSongDto>());

            var json = await response.Content.ReadAsStringAsync();
            var songs = JsonSerializer.Deserialize<List<ResultSongDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(songs);
        }

        [HttpGet]
        public async Task<IActionResult> CreateSong()
        {
            var client = _httpClientFactory.CreateClient();

            // Paketleri API'den çek
            var response = await client.GetAsync("https://localhost:7157/api/Package");
            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Packages = new List<object>();
                return View();
            }

            var json = await response.Content.ReadAsStringAsync();
            var packages = JsonSerializer.Deserialize<List<ResultPackageDto>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            ViewBag.Packages = packages;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSong(CreateSongDto dto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonSerializer.Serialize(dto);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7157/api/Song", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError("", "Şarkı eklenemedi!");
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateSong(int id)
        {
            var client2 = _httpClientFactory.CreateClient();

            // Paketleri API'den çek
            var response2 = await client2.GetAsync("https://localhost:7157/api/Package");
            if (!response2.IsSuccessStatusCode)
            {
                ViewBag.Packages = new List<object>();
                return View();
            }

            var json2 = await response2.Content.ReadAsStringAsync();
            var packages = JsonSerializer.Deserialize<List<ResultPackageDto>>(json2,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            ViewBag.Packages = packages;           

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7157/api/Song/{id}");

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var song = JsonSerializer.Deserialize<UpdateSongDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(song);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSong(UpdateSongDto dto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonSerializer.Serialize(dto);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PutAsync("https://localhost:7157/api/Song", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError("", "Şarkı güncellenemedi!");
            return View(dto);
        }

        public async Task<IActionResult> DeleteSong(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"https://localhost:7157/api/Song/{id}");

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            TempData["Error"] = "Şarkı silinemedi!";
            return RedirectToAction("Index");
        }
    }
}
