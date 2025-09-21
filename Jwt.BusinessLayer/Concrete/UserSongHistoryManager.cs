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
    public class UserSongHistoryManager : GenericManager<UserSongHistory>, IUserSongHistoryService
    {
        private readonly IUserSongHistoryDal _userSongHistoryDal;

        public UserSongHistoryManager(IUserSongHistoryDal userSongHistoryDal) : base(userSongHistoryDal)
        {
            _userSongHistoryDal = userSongHistoryDal;
        }

        public List<UserSongHistory> TGetAllWithSong()
        {
            return _userSongHistoryDal.GetAllWithSong();
        }

        public List<int> TGetMonthlyListeningStats()
        {
            return _userSongHistoryDal.GetMonthlyListeningStats();
        }

        public Song TGetMostListenedSong()
        {
            return _userSongHistoryDal.GetMostListenedSong();
        }

        public int TGetTotalListeningCount()
        {
            return _userSongHistoryDal.GetTotalListeningCount();
        }       

        public List<UserSongHistory> TGetUserSongHistoriesWithUserAndSong()
        {
            return _userSongHistoryDal.GetUserSongHistoriesWithUserAndSong();
        }

        public Task TIncrementListenCountAsync(int userId, int songId)
        {
            return _userSongHistoryDal.IncrementListenCountAsync(userId, songId);
        }
    }
}
