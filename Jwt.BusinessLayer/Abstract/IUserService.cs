using Jwt.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.BusinessLayer.Abstract
{
    public interface IUserService : IGenericService<User>
    {
        User GetByEmail(string email);

        public int TGetUserPackageIdByUserId(int id);

        public List<User> TGetUserListWithPackages();

        public int TGetTotalUserCount();
    }
}
