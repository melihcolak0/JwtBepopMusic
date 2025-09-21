using Jwt.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.DataAccessLayer.Abstract
{
    public interface ISongDal : IGenericDal<Song>
    {
        public List<Song> ListSongsByPackageLevel(int userPackageId);

        public List<Song> List8MostPopularSongsByPackageLevel(int userPackageId);

        public List<Song> ListLast5SongsByPackageLevel(int userPackageId);

        public List<Song> ListLast10SongsAWeekByPackageLevel(int userPackageId);

        public List<Song> List5SongsByPackageLevelAndArtist(int userPackageId, string artist);

        public List<Song> ListSongsWithPackages();

        public int GetTotalSongCount();

        Task<IEnumerable<Song>> GetTopRecommendedSongsAsync(int count);
    }
}
