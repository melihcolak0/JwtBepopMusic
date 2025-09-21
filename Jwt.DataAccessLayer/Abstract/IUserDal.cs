using Jwt.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.DataAccessLayer.Abstract
{
    public interface IUserDal : IGenericDal<User>
    {
        public int GetUserPackageIdByUserId(int id);

        public List<User> GetUserListWithPackages();     

        public int GetTotalUserCount();

        Task<User> GetUserWithRatingsAsync(int id);
    }
}
