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
    public class EfUserDal : EfGenericRepository<User>, IUserDal
    {
        private readonly JwtContext _context;

        public EfUserDal(JwtContext context) : base(context)
        {
            _context = context;
        }

        public int GetTotalUserCount()
        {
            return _context.Users.Count();
        }

        public List<User> GetUserListWithPackages()
        {
            return _context.Users.Include(x=> x.Package).ToList();
        }

        public int GetUserPackageIdByUserId(int id)
        {
            return _context.Users.Where(x => x.Id == id).Select(y => y.PackageId).FirstOrDefault();
        }

        public async Task<User> GetUserWithRatingsAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Recommendations)
                .ThenInclude(r => r.Song)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
