using Jwt.PresentationLayer.Dtos;
using Jwt.PresentationLayer.Dtos.SongDtos;
using Jwt.PresentationLayer.Dtos.UserSongHistoryDtos;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace Jwt.PresentationLayer.Controllers
{
    public class AdminUserSongHistoryController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminUserSongHistoryController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        //public async Task<IActionResult> Index()
        //{
        //    var client = _httpClientFactory.CreateClient();
        //    var response = await client.GetAsync("https://localhost:7157/api/UserSongHistory/GetUserSongHistoriesWithUserAndSong");

        //    if (!response.IsSuccessStatusCode)
        //        return View(new List<ResultUserSongHistoryDto>());

        //    var json = await response.Content.ReadAsStringAsync();
        //    var values = JsonSerializer.Deserialize<List<ResultUserSongHistoryDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        //    return View(values);
        //}

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7157/api/UserSongHistory/GetPagedList?page={page}&pageSize={pageSize}");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Veriler alınamadı!";
                return View(new List<ResultUserSongHistoryDto>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var pagedData = JsonSerializer.Deserialize<PagedResultDto<ResultUserSongHistoryDto>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = pagedData.TotalPages;

            return View(pagedData);
        }

        [HttpGet]
        public async Task<IActionResult> CreateUserSongHistory()
        {
            var client = _httpClientFactory.CreateClient();

            var userResponse = await client.GetAsync("https://localhost:7157/api/User");
            var userJson = await userResponse.Content.ReadAsStringAsync();
            var users = JsonSerializer.Deserialize<List<ResultUserDto>>(userJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            ViewBag.Users = users;

            var songResponse = await client.GetAsync("https://localhost:7157/api/Song");
            var songJson = await songResponse.Content.ReadAsStringAsync();
            var songs = JsonSerializer.Deserialize<List<ResultSongDto>>(songJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            ViewBag.Songs = songs;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserSongHistory(CreateUserSongHistoryDto dto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonSerializer.Serialize(dto);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7157/api/UserSongHistory", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError("", "Ekleme başarısız!");
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateUserSongHistory(int id)
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetAsync($"https://localhost:7157/api/UserSongHistory/{id}");
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var history = JsonSerializer.Deserialize<UpdateUserSongHistoryDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var userResponse = await client.GetAsync("https://localhost:7157/api/User");
            var userJson = await userResponse.Content.ReadAsStringAsync();
            var users = JsonSerializer.Deserialize<List<ResultUserDto>>(userJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            ViewBag.Users = users;

            var songResponse = await client.GetAsync("https://localhost:7157/api/Song");
            var songJson = await songResponse.Content.ReadAsStringAsync();
            var songs = JsonSerializer.Deserialize<List<ResultSongDto>>(songJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            ViewBag.Songs = songs;

            return View(history);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserSongHistory(UpdateUserSongHistoryDto dto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonSerializer.Serialize(dto);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PutAsync("https://localhost:7157/api/UserSongHistory", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError("", "Güncelleme başarısız!");
            return View(dto);
        }

        public async Task<IActionResult> DeleteUserSongHistory(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"https://localhost:7157/api/UserSongHistory/{id}");

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            TempData["Error"] = "Silme işlemi başarısız!";
            return RedirectToAction("Index");
        }
    }
}
