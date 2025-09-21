using Jwt.PresentationLayer.Dtos.SongDtos;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace Jwt.PresentationLayer.ViewComponents.DefaultComponents
{
    public class _MostPopularSongsDefaultComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _MostPopularSongsDefaultComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Giriş Yapan Kullanıcının UserId'sini Bulma
            string userId = null;
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);
                userId = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            }

            // Giriş Yapan Kullanıcının UserId'sini Bulma
            int userPackageId = 0; // default (en düşük paket)

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

            // Song List Consume
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetAsync("https://localhost:7157/api/Song/GetList8MostPopularSongs/" + userPackageId);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Popüler şarkılar alınamadı!";
                return View(new List<ResultSongDto>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var songs = JsonSerializer.Deserialize<List<ResultSongDto>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(songs);
        }
    }
}
