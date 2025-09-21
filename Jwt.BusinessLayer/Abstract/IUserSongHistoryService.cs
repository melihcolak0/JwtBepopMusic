using Jwt.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.BusinessLayer.Abstract
{
    public interface IUserSongHistoryService : IGenericService<UserSongHistory>
    {
        public List<UserSongHistory> TGetUserSongHistoriesWithUserAndSong();

        public int TGetTotalListeningCount();

        public Song TGetMostListenedSong();

        public List<int> TGetMonthlyListeningStats();

        public List<UserSongHistory> TGetAllWithSong();

        public Task TIncrementListenCountAsync(int userId, int songId);
    }
}
