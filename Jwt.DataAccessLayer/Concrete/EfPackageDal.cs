using Jwt.DataAccessLayer.Abstract;
using Jwt.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.DataAccessLayer.Concrete
{
    public class EfPackageDal : EfGenericRepository<Package>, IPackageDal
    {
        public EfPackageDal(JwtContext context) : base(context)
        {
        }
    }
}
