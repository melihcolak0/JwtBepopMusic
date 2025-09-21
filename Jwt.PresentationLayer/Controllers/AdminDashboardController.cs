//using Jwt.PresentationLayer.Dtos.ArtistDtos;
//using Jwt.PresentationLayer.Dtos.SongDtos;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;
//using System.Text.Json;
//using System.Threading.Tasks;

//namespace Jwt.PresentationLayer.Controllers
//{
//    public class AdminDashboardController : Controller
//    {
//        private readonly IHttpClientFactory _httpClientFactory;

//        public AdminDashboardController(IHttpClientFactory httpClientFactory)
//        {
//            _httpClientFactory = httpClientFactory;
//        }

//        public async Task<IActionResult> Index()
//        {
//            var client = _httpClientFactory.CreateClient();

//            // --- Tüm Şarkılar ---
//            var response = await client.GetAsync("https://localhost:7157/api/Song");
//            List<ResultSongDto> allSongs = new();
//            if (response.IsSuccessStatusCode)
//            {
//                var json = await response.Content.ReadAsStringAsync();
//                if (!string.IsNullOrEmpty(json) && json != "null")
//                {
//                    allSongs = JsonSerializer.Deserialize<List<ResultSongDto>>(json,
//                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
//                }
//            }

//            // --- Benzersiz Sanatçılar + Resim ---
//            var artists = allSongs
//                .Where(x => !string.IsNullOrEmpty(x.Artist))
//                .GroupBy(x => x.Artist.Trim(), StringComparer.OrdinalIgnoreCase)
//                .Select(g => new ResultArtistDto
//                {
//                    Artist = g.Key,
//                    ArtistImageUrl = g.FirstOrDefault()?.ArtistImageUrl
//                })
//                .OrderBy(x => x.Artist)
//                .ToList();

//            ViewBag.Artists = artists;
//            ViewBag.ArtistCount = artists.Count;

//            // --- Diğer İstatistikler ---
//            // Toplam kullanıcı, toplam şarkı, toplam dinlenme, en çok dinlenen şarkı gibi
//            var userResponse = await client.GetAsync("https://localhost:7157/api/User/GetTotalUserCount");
//            int totalUsers = 0;
//            if (userResponse.IsSuccessStatusCode)
//            {
//                var value = await userResponse.Content.ReadAsStringAsync();
//                totalUsers = int.Parse(value);
//            }

//            var songResponse = await client.GetAsync("https://localhost:7157/api/Song/GetTotalSongCount");
//            int totalSongs = 0;
//            if (songResponse.IsSuccessStatusCode)
//            {
//                var value = await songResponse.Content.ReadAsStringAsync();
//                totalSongs = int.Parse(value);
//            }

//            var listeningResponse = await client.GetAsync("https://localhost:7157/api/UserSongHistory/GetTotalListeningCount");
//            int totalListening = 0;
//            if (listeningResponse.IsSuccessStatusCode)
//            {
//                var value = await listeningResponse.Content.ReadAsStringAsync();
//                totalListening = int.Parse(value);
//            }

//            var mostListenedResponse = await client.GetAsync("https://localhost:7157/api/UserSongHistory/GetMostListenedSong");
//            string mostListenedSong = "Bulunamadı";
//            if (mostListenedResponse.IsSuccessStatusCode)
//            {
//                var jsonSong = await mostListenedResponse.Content.ReadAsStringAsync();
//                if (!string.IsNullOrEmpty(jsonSong) && jsonSong != "null")
//                {
//                    var song = JsonSerializer.Deserialize<ResultSongDto>(jsonSong,
//                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
//                    if (song != null)
//                    {
//                        mostListenedSong = $"{song.Artist} - {song.Title}";
//                    }
//                }
//            }

//            ViewBag.TotalUsers = totalUsers;
//            ViewBag.TotalSongs = totalSongs;
//            ViewBag.TotalListening = totalListening;
//            ViewBag.MostListenedSong = mostListenedSong;

//            var mostPopular8Songs = await GetMostPopular8Songs();

//            return View(mostPopular8Songs);
//        }

//        public async Task<List<ResultSongDto>> GetMostPopular8Songs()
//        {
//            // Giriş Yapan Kullanıcının UserId'sini Bulma
//            string userId = null;
//            var token = HttpContext.Session.GetString("JWToken");
//            if (!string.IsNullOrEmpty(token))
//            {
//                var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
//                var jwt = handler.ReadJwtToken(token);
//                userId = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
//            }

//            // Giriş Yapan Kullanıcının UserId'sini Bulma
//            int userPackageId = 0; // default (en düşük paket)

//            if (!string.IsNullOrEmpty(userId))
//            {
//                var client2 = _httpClientFactory.CreateClient();
//                var userResponse = await client2.GetAsync($"https://localhost:7157/api/User/GetUserPackageIdByUserId/{userId}");

//                if (userResponse.IsSuccessStatusCode)
//                {
//                    var userJson = await userResponse.Content.ReadAsStringAsync();
//                    // Diyelim ki API sadece int dönüyor
//                    userPackageId = JsonSerializer.Deserialize<int>(userJson);
//                }
//            }

//            // Song List Consume
//            var client = _httpClientFactory.CreateClient();

//            var response = await client.GetAsync("https://localhost:7157/api/Song/GetList8MostPopularSongs/" + userPackageId);

//            if (!response.IsSuccessStatusCode)
//            {
//                ViewBag.Error = "Popüler şarkılar alınamadı!";
//                return new List<ResultSongDto>();
//            }

//            var json = await response.Content.ReadAsStringAsync();
//            var songs = JsonSerializer.Deserialize<List<ResultSongDto>>(json,
//                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

//            return songs;
//        }

//    }
//}

using Jwt.PresentationLayer.Dtos.ArtistDtos;
using Jwt.PresentationLayer.Dtos.SongDtos;
using Jwt.PresentationLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Jwt.PresentationLayer.Controllers
{
    public class AdminDashboardController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminDashboardController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();

            var viewModel = new AdminDashboardViewModel();

            // --- Tüm Şarkılar ---
            var response = await client.GetAsync("https://localhost:7157/api/Song");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(json) && json != "null")
                {
                    viewModel.AllSongs = JsonSerializer.Deserialize<List<ResultSongDto>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }
            else
            {
                viewModel.AllSongs = new List<ResultSongDto>();
            }

            // --- Benzersiz Sanatçılar + Resim ---
            viewModel.Artists = viewModel.AllSongs
                .Where(x => !string.IsNullOrEmpty(x.Artist))
                .GroupBy(x => x.Artist.Trim(), StringComparer.OrdinalIgnoreCase)
                .Select(g => new ResultArtistDto
                {
                    Artist = g.Key,
                    ArtistImageUrl = g.FirstOrDefault()?.ArtistImageUrl
                })
                .OrderBy(x => x.Artist)
                .ToList();

            // --- Diğer İstatistikler ---
            viewModel.TotalUsers = await GetIntFromApi(client, "https://localhost:7157/api/User/GetTotalUserCount");
            viewModel.TotalSongs = await GetIntFromApi(client, "https://localhost:7157/api/Song/GetTotalSongCount");
            viewModel.TotalListening = await GetIntFromApi(client, "https://localhost:7157/api/UserSongHistory/GetTotalListeningCount");

            viewModel.MostListenedSong = await GetMostListenedSong(client);

            // --- 8 Popüler Şarkı ---
            viewModel.MostPopularSongs = await GetMostPopular8Songs();

            return View(viewModel);
        }

        private async Task<int> GetIntFromApi(HttpClient client, string url)
        {
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var value = await response.Content.ReadAsStringAsync();
                if (int.TryParse(value, out int result))
                    return result;
            }
            return 0;
        }

        private async Task<string> GetMostListenedSong(HttpClient client)
        {
            var response = await client.GetAsync("https://localhost:7157/api/UserSongHistory/GetMostListenedSong");
            if (response.IsSuccessStatusCode)
            {
                var jsonSong = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(jsonSong) && jsonSong != "null")
                {
                    var song = JsonSerializer.Deserialize<ResultSongDto>(jsonSong,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (song != null)
                    {
                        return $"{song.Artist} - {song.Title}";
                    }
                }
            }
            return "Bulunamadı";
        }

        public async Task<List<ResultSongDto>> GetMostPopular8Songs()
        {           
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7157/api/Song/GetList8MostPopularSongs/6");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var songs = JsonSerializer.Deserialize<List<ResultSongDto>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return songs ?? new List<ResultSongDto>();
            }

            return new List<ResultSongDto>();
        }
    }
}

