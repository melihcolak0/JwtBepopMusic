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
    public class EfUserSongHistoryDal : EfGenericRepository<UserSongHistory>, IUserSongHistoryDal
    {
        private readonly JwtContext _context;

        public EfUserSongHistoryDal(JwtContext context) : base(context)
        {
            _context = context;
        }

        public List<UserSongHistory> GetAllWithSong()
        {
            return _context.UserSongHistories.Include(x => x.Song).ToList();
        }

        public List<int> GetMonthlyListeningStats()
        {
            var now = DateTime.Now;
            var last12Months = Enumerable.Range(0, 12)
                .Select(i => now.AddMonths(-i))
                .OrderBy(d => d)
                .ToList();

            var monthlyCounts = new List<int>();

            foreach (var month in last12Months)
            {
                int count = _context.UserSongHistories
                    .Where(x => x.LastListened.Month == month.Month && x.LastListened.Year == month.Year)
                    .Sum(x => x.ListenCount);

                monthlyCounts.Add(count);
            }

            return monthlyCounts;
        }

        public Song GetMostListenedSong()
        {
            var result = _context.UserSongHistories
            .GroupBy(x => x.SongId)
            .Select(g => new
            {
                SongId = g.Key,
                TotalListen = g.Sum(x => x.ListenCount)
            })
            .OrderByDescending(x => x.TotalListen)
            .FirstOrDefault();

            if (result == null) return null;

            return _context.Songs.FirstOrDefault(s => s.Id == result.SongId);
        }

        public int GetTotalListeningCount()
        {
            return _context.UserSongHistories.Sum(x => x.ListenCount);
        }

        

        public List<UserSongHistory> GetUserSongHistoriesWithUserAndSong()
        {
            return _context.UserSongHistories.Include(x => x.Song).Include(y => y.User).ToList();
        }

        public async Task IncrementListenCountAsync(int userId, int songId)
        {
            var history = await _context.UserSongHistories
            .FirstOrDefaultAsync(h => h.UserId == userId && h.SongId == songId);

            if (history == null)
            {
                // Kullanýcý bu þarkýyý ilk defa dinliyor
                history = new UserSongHistory
                {
                    UserId = userId,
                    SongId = songId,
                    ListenCount = 1,
                    LastListened = DateTime.Now
                };
                await _context.UserSongHistories.AddAsync(history);
            }
            else
            {
                // Daha önce dinlemiþ sayacý artýr
                history.ListenCount++;
                history.LastListened = DateTime.Now;
                _context.UserSongHistories.Update(history);
            }

            await _context.SaveChangesAsync();
        }
    }
}
