using Jwt.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.DataAccessLayer.Abstract
{
    public interface IUserSongHistoryDal : IGenericDal<UserSongHistory>
    {
        public List<UserSongHistory> GetUserSongHistoriesWithUserAndSong();

        public int GetTotalListeningCount();

        public Song GetMostListenedSong();

        public List<int> GetMonthlyListeningStats();

        public List<UserSongHistory> GetAllWithSong();

        public Task IncrementListenCountAsync(int userId, int songId);
    }
}
