using Jwt.DataAccessLayer.Abstract;
using Jwt.EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.DataAccessLayer.Concrete
{
    public class EfRecommendationDal : EfGenericRepository<Recommendation>, IRecommendationDal
    {
        private readonly JwtContext _context;

        public EfRecommendationDal(JwtContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Recommendation>> GetRecommendationsByUserIdAsync(int userId)
        {
            return await _context.Recommendations
                .Where(r => r.UserId == userId)
                .Include(r => r.Song)
                .ToListAsync();
        }
    }
}
