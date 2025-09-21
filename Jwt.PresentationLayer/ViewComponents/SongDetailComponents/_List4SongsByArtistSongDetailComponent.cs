using Jwt.PresentationLayer.Dtos.SongDtos;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Jwt.PresentationLayer.ViewComponents.SongDetailComponents
{
    public class _List4SongsByArtistSongDetailComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _List4SongsByArtistSongDetailComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync(int userPackageId, string artist, int? excludeSongId = null)
        {
            var client = _httpClientFactory.CreateClient();

            // artist parametresini URL encode ediyoruz
            var encodedArtist = Uri.EscapeDataString(artist);

            var response = await client.GetAsync($"https://localhost:7157/api/Song/GetList5SongsByPackageLevelAndArtist/{userPackageId}/{encodedArtist}");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Şarkılar alınamadı!";
                return View(new List<ResultSongDto>());
            }

            var json = await response.Content.ReadAsStringAsync();

            var songs = JsonSerializer.Deserialize<List<ResultSongDto>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (excludeSongId.HasValue && excludeSongId.Value > 0)
            {
                songs = songs.Where(x => x.Id != excludeSongId.Value).ToList();
            }

            return View(songs);
        }
    }
}
