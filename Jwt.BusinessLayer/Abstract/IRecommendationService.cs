using Jwt.DtoLayer.RecommendationDtos;
using Jwt.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.BusinessLayer.Abstract
{
    public interface IRecommendationService : IGenericService<Recommendation>
    {
        Task<IEnumerable<ResultRecommendationDto>> GetRecommendationsAsync(int userId, int count = 5);
    }
}
