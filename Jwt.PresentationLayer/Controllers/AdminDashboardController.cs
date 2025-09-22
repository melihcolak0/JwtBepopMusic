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

