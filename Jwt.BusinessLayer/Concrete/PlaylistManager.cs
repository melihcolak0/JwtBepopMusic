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
    public class PlaylistManager : GenericManager<Playlist>, IPlaylistService
    {
        private readonly IPlaylistDal _playlistDal;

        public PlaylistManager(IPlaylistDal playlistDal) : base(playlistDal)
        {
            _playlistDal = playlistDal;
        }
    }
}
