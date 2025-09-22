using Jwt.PresentationLayer.Dtos.SongDtos;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace Jwt.PresentationLayer.Controllers
{
    public class ChartsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ChartsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index(string? genre)
        {
            string userId = null;
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

            int userPackageId = 1;
            if (!string.IsNullOrEmpty(userId))
            {
                var client2 = _httpClientFactory.CreateClient();
                var userResponse = await client2.GetAsync($"https://localhost:7157/api/User/GetUserPackageIdByUserId/{userId}");

                if (userResponse.IsSuccessStatusCode)
                {
                    var userJson = await userResponse.Content.ReadAsStringAsync();
                    userPackageId = JsonSerializer.Deserialize<int>(userJson);
                }
            }
            ViewBag.UserPackageId = userPackageId;

            // Haftanın en çok dinlenen son 10 şarkısı
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7157/api/Song/GetListLast10SongsAWeekByPackageLevel/{userPackageId}");
            var json = await response.Content.ReadAsStringAsync();
            var songs = JsonSerializer.Deserialize<List<ResultSongDto>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Genre listesi
            var genres = songs
                .Where(x => !string.IsNullOrEmpty(x.Genre))
                .Select(x => x.Genre.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x)
                .ToList();

            genres.Insert(0, "Hepsi");
            ViewBag.Genres = genres;

            if (!string.IsNullOrEmpty(genre) && genre != "Hepsi")
            {
                songs = songs
                    .Where(x => !string.IsNullOrEmpty(x.Genre) &&
                                x.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            ViewBag.SelectedGenre = genre ?? "Hepsi";

            return View(songs);
        }
    }
}
