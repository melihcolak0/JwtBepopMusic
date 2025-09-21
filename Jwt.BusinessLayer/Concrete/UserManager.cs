using Jwt.BusinessLayer.Abstract;
using Jwt.DataAccessLayer.Abstract;
using Jwt.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.BusinessLayer.Concrete
{
    public class UserManager : GenericManager<User>, IUserService
    {
        private readonly IUserDal _userDal;

        public UserManager(IUserDal userDal) : base(userDal)
        {
            _userDal = userDal;
        }

        public User GetByEmail(string email)
        {
            return _userDal.GetAll(u => u.Email == email).FirstOrDefault();
        }

        public int TGetTotalUserCount()
        {
            return _userDal.GetTotalUserCount();
        }

        public List<User> TGetUserListWithPackages()
        {
            return _userDal.GetUserListWithPackages();
        }

        public int TGetUserPackageIdByUserId(int id)
        {
            return _userDal.GetUserPackageIdByUserId(id);
        }
    }
}
