using Jwt.BusinessLayer.Abstract;
using Jwt.DtoLayer.RecommendationDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _12PC_Jwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationController : ControllerBase
    {
        private readonly IRecommendationService _recommendationService;


        public RecommendationController(IRecommendationService recommendationService)
        {
            _recommendationService = recommendationService;
        }

        // GET: api/recommendation/{userId}?count=5
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<ResultRecommendationDto>>> GetRecommendations(int userId, int count = 5)
        {
            var recommendations = await _recommendationService.GetRecommendationsAsync(userId, count);

            if (recommendations == null)
                return NotFound("No recommendations found.");

            return Ok(recommendations);
        }        
    }
}
