using Jwt.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.BusinessLayer.Abstract
{
    public interface ISongService : IGenericService<Song>
    {
        public List<Song> TListSongsByPackageLevel(int userPackageId);

        public List<Song> TList8MostPopularSongsByPackageLevel(int userPackageId);

        public List<Song> TListLast5SongsByPackageLevel(int userPackageId);

        public List<Song> TListLast10SongsAWeekByPackageLevel(int userPackageId);

        public List<Song> TList5SongsByPackageLevelAndArtist(int userPackageId, string artist);

        public List<Song> TListSongsWithPackages();

        public int TGetTotalSongCount();
    }
}
