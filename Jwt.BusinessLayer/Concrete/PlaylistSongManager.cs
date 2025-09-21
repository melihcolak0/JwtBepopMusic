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
    public class PlaylistSongManager : GenericManager<PlaylistSong>, IPlaylistSongService
    {
        private readonly IPlaylistSongDal _playlistSongDal;

        public PlaylistSongManager(IPlaylistSongDal playlistSongDal) : base(playlistSongDal)
        {
            _playlistSongDal = playlistSongDal;
        }
    }
}
