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
    public class SongManager : GenericManager<Song>, ISongService
    {
        private readonly ISongDal _songDal;

        public SongManager(ISongDal songDal) : base(songDal)
        {
            _songDal = songDal;
        }

        public int TGetTotalSongCount()
        {
            return _songDal.GetTotalSongCount();
        }

        public List<Song> TList5SongsByPackageLevelAndArtist(int userPackageId, string artist)
        {
            return _songDal.List5SongsByPackageLevelAndArtist(userPackageId, artist);
        }

        public List<Song> TList8MostPopularSongsByPackageLevel(int userPackageId)
        {
            return _songDal.List8MostPopularSongsByPackageLevel(userPackageId);
        }

        public List<Song> TListLast10SongsAWeekByPackageLevel(int userPackageId)
        {
            return _songDal.ListLast10SongsAWeekByPackageLevel(userPackageId);
        }

        public List<Song> TListLast5SongsByPackageLevel(int userPackageId)
        {
            return _songDal.ListLast5SongsByPackageLevel(userPackageId);
        }

        public List<Song> TListSongsByPackageLevel(int userPackageId)
        {
            return _songDal.ListSongsByPackageLevel(userPackageId);
        }

        public List<Song> TListSongsWithPackages()
        {
            return _songDal.ListSongsWithPackages();
        }
    }
}
