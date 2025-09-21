using Jwt.PresentationLayer.Dtos.ArtistDtos;
using Jwt.PresentationLayer.Dtos.SongDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Jwt.PresentationLayer.Controllers
{
    [Authorize]
    public class ArtistsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ArtistsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetAsync("https://localhost:7157/api/Song");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Şarkılar alınamadı!";
                return View(new List<ResultSongDto>());
            }

            var json = await response.Content.ReadAsStringAsync();

            var songs = JsonSerializer.Deserialize<List<ResultSongDto>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // 🔹 Benzersiz Artist + Resim listesi
            var artists = songs
                .Where(x => !string.IsNullOrEmpty(x.Artist))
                .GroupBy(x => x.Artist.Trim(), StringComparer.OrdinalIgnoreCase)
                .Select(g => new ResultArtistDto
                {
                    Artist = g.Key,
                    ArtistImageUrl = g.FirstOrDefault()?.ArtistImageUrl
                })
                .OrderBy(x => x.Artist)
                .ToList();

            ViewBag.Artists = artists;

            int artistCount = artists.Count;
            ViewBag.ArtistCount = artistCount;

            return View(songs);
        }

        public async Task<IActionResult> GetArtistByName([FromQuery] string artist)
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

            if (string.IsNullOrEmpty(artist))
            {
                return BadRequest("Artist ismi boş olamaz!");
            }


            var client = _httpClientFactory.CreateClient();

            // Tüm şarkıları çekiyoruz
            var response = await client.GetAsync("https://localhost:7157/api/Song");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Şarkılar alınamadı!";
                return View(new List<ResultSongDto>());
            }

            var json = await response.Content.ReadAsStringAsync();

            var songs = JsonSerializer.Deserialize<List<ResultSongDto>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (songs == null || !songs.Any())
            {
                ViewBag.Error = "Şarkı bulunamadı!";
                return View(new List<ResultSongDto>());
            }

            // Artist ismine göre filtreleme
            var selectedArtist = songs
                .Where(x => !string.IsNullOrEmpty(x.Artist) &&
                            x.Artist.Equals(artist, StringComparison.OrdinalIgnoreCase))
                .GroupBy(x => x.Artist.Trim(), StringComparer.OrdinalIgnoreCase)
                .Select(g => new ResultArtistDto
                {
                    Artist = g.Key,
                    ArtistImageUrl = g.FirstOrDefault()?.ArtistImageUrl
                })
                .FirstOrDefault();

            if (selectedArtist == null)
            {
                ViewBag.Error = "Artist bulunamadı!";
                return View(new List<ResultArtistDto>());
            }

            return View(selectedArtist);
        }
    }
}
