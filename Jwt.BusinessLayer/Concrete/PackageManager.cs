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
    public class PackageManager : GenericManager<Package>, IPackageService
    {
        private readonly IPackageDal _packageDal;

        public PackageManager(IPackageDal packageDal) : base(packageDal)
        {
            _packageDal = packageDal;
        }
    }
}
