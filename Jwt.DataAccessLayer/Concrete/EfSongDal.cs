using Jwt.DataAccessLayer.Abstract;
using Jwt.EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.DataAccessLayer.Concrete
{
    public class EfSongDal : EfGenericRepository<Song>, ISongDal
    {
        private readonly JwtContext _context;

        public EfSongDal(JwtContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Song>> GetTopRecommendedSongsAsync(int count)
        {
            return await _context.Songs
                .OrderByDescending(s => s.Recommendations.Average(r => r.Score))
                .Take(count)
                .ToListAsync();
        }

        public int GetTotalSongCount()
        {
            return _context.Songs.Count();
        }

        public List<Song> List5SongsByPackageLevelAndArtist(int userPackageId, string artist)
        {
            return _context.Songs
                .Where(s => s.Artist == artist && s.PackageId <= userPackageId)
                .OrderByDescending(s => s.ReleaseDate)
                .Take(4)
                .ToList();
        }

        public List<Song> List8MostPopularSongsByPackageLevel(int userPackageId)
        {
            var songs = _context.UserSongHistories
                .GroupBy(h => h.SongId)
                .Select(g => new
                {
                    SongId = g.Key,
                    TotalListen = g.Sum(x => x.ListenCount)
                })
                .OrderByDescending(x => x.TotalListen)
                .Join(_context.Songs,
                    h => h.SongId,
                    s => s.Id,
                    (h, s) => s)
                .Where(s => s.PackageId <= userPackageId)
                .Take(8)
                .ToList();

            return songs;
        }

        public List<Song> ListLast10SongsAWeekByPackageLevel(int userPackageId)
        {
            var oneWeekAgo = DateTime.Now.AddDays(-7);

            var songs = _context.UserSongHistories
                .Where(h => h.LastListened >= oneWeekAgo)
                .GroupBy(h => h.SongId)
                .Select(g => new
                {
                    SongId = g.Key,
                    TotalListen = g.Sum(x => x.ListenCount)
                })
                .OrderByDescending(x => x.TotalListen)
                .Take(10)
                .Join(_context.Songs,
                    h => h.SongId,
                    s => s.Id,
                    (h, s) => s)
                .Where(s => s.PackageId <= userPackageId)
                .ToList();

            return songs;
        }

        public List<Song> ListLast5SongsByPackageLevel(int userPackageId)
        {
            var songs = _context.Songs
                .Where(s => s.PackageId <= userPackageId)
                .OrderByDescending(s => s.ReleaseDate)
                .Take(5)
                .ToList();

            return songs;
        }

        public List<Song> ListSongsByPackageLevel(int userPackageId)
        {
            return _context.Songs
                     .Where(s => s.PackageId <= userPackageId)
                     .OrderBy(s => s.PackageId)
                     .ToList();
        }

        public List<Song> ListSongsWithPackages()
        {
            return _context.Songs
                .Include(s => s.Package)
                .OrderBy(s => s.Id)
                .ToList();
        }
    }
}
