using Jwt.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.DataAccessLayer.Abstract
{
    public interface IRecommendationDal : IGenericDal<Recommendation>
    {
        Task<IEnumerable<Recommendation>> GetRecommendationsByUserIdAsync(int userId);
    }
}
