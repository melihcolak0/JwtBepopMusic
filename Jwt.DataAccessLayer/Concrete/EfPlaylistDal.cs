using Jwt.DataAccessLayer.Abstract;
using Jwt.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.DataAccessLayer.Concrete
{
    public class EfPlaylistDal : EfGenericRepository<Playlist>, IPlaylistDal
    {
        public EfPlaylistDal(JwtContext context) : base(context)
        {
        }
    }
}
