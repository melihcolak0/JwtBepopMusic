using Humanizer.Localisation;
using Jwt.PresentationLayer.Dtos.SongDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace Jwt.PresentationLayer.Controllers
{
    [Authorize]
    public class DefaultController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DefaultController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);

                string userId = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                string username = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                string role = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                ViewBag.UserId = userId;
                ViewBag.UserName = username;
                ViewBag.UserRole = role;
            }
            return View();
        }        

        //public async Task<IActionResult> Genres(string? genre)
        //{
        //    string userId = null;
        //    //Jwt Token
        //    var token = HttpContext.Session.GetString("JWToken");
        //    if (!string.IsNullOrEmpty(token))
        //    {
        //        var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        //        var jwt = handler.ReadJwtToken(token);

        //        userId = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        //        string username = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        //        string role = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        //        ViewBag.UserId = userId;
        //        ViewBag.UserName = username;
        //        ViewBag.UserRole = role;
        //    }

        //    // UserPackageId Consume
        //    int userPackageId = 1; // default (en düşük paket)

        //    if (!string.IsNullOrEmpty(userId))
        //    {
        //        var client2 = _httpClientFactory.CreateClient();
        //        var userResponse = await client2.GetAsync($"https://localhost:7157/api/User/GetUserPackageIdByUserId/{userId}");

        //        if (userResponse.IsSuccessStatusCode)
        //        {
        //            var userJson = await userResponse.Content.ReadAsStringAsync();
        //            // Diyelim ki API sadece int dönüyor
        //            userPackageId = JsonSerializer.Deserialize<int>(userJson);
        //        }
        //    }
        //    ViewBag.UserPackageId = userPackageId;

        //    // Song API Consume
        //    var client = _httpClientFactory.CreateClient();

        //    var allSongsResponse = await client.GetAsync($"https://localhost:7157/api/Song/GetListByPackageLevel/" + userPackageId);
        //    var allSongsJson = await allSongsResponse.Content.ReadAsStringAsync();
        //    var allSongs = JsonSerializer.Deserialize<List<ResultSongDto>>(allSongsJson,
        //        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        //    // Tüm şarkılardan tür listesi oluştur
        //    var genres = allSongs
        //        .Where(x => !string.IsNullOrEmpty(x.Genre))
        //        .Select(x => x.Genre.Trim())
        //        .Distinct(StringComparer.OrdinalIgnoreCase)
        //        .OrderBy(x => x)
        //        .ToList();

        //    genres.Insert(0, "Hepsi");
        //    ViewBag.Genres = genres;

        //    // Şarkıları filtrele
        //    if (!string.IsNullOrEmpty(genre) && genre != "Hepsi")
        //    {
        //        allSongs = allSongs
        //            .Where(x => !string.IsNullOrEmpty(x.Genre) &&
        //                        x.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase))
        //            .ToList();
        //    }

        //    ViewBag.SelectedGenre = genre ?? "Hepsi";

        //    return View(allSongs);
        //}
    }
}
