using Jwt.PresentationLayer.Dtos;
using Jwt.PresentationLayer.Dtos.PackageDtos;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Jwt.PresentationLayer.Controllers
{
    public class AdminUserController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminUserController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7157/api/User/GetUserListWithPackages");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Paketler alınamadı!";
                return View(new List<ResultUserDto>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var values = JsonSerializer.Deserialize<List<ResultUserDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(values);
        }

        public async Task<IActionResult> DeleteUser(int id)
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.DeleteAsync($"https://localhost:7157/api/User/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Kullanıcı başarıyla silindi!";
            }
            else
            {
                TempData["Error"] = "Silme işlemi başarısız!";
            }

            return RedirectToAction("Index");
        }

    }
}
