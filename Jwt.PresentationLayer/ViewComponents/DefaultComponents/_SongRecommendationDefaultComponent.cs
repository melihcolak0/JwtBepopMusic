using Jwt.PresentationLayer.Dtos.RecommendationDtos;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Jwt.PresentationLayer.ViewComponents.DefaultComponents
{
    public class _SongRecommendationDefaultComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _SongRecommendationDefaultComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync(int count = 7)
        {
            string userId = null;
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);
                userId = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            }

            var client = _httpClientFactory.CreateClient();

            // API endpoint
            var apiUrl = $"https://localhost:7157/api/recommendation/{userId}?count={count}";

            IEnumerable<ResultRecommendationDto> recommendations;

            try
            {
                // API'den veri çek
                recommendations = await client.GetFromJsonAsync<IEnumerable<ResultRecommendationDto>>(apiUrl);
            }
            catch
            {
                // Hata durumunda boş liste döndür
                recommendations = new List<ResultRecommendationDto>();
            }            

            return View(recommendations);
        }
    }
}
