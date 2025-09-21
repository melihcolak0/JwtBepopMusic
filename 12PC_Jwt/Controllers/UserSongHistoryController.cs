using Jwt.BusinessLayer.Abstract;
using Jwt.DtoLayer;
using Jwt.DtoLayer.UserSongHistoryDtos;
using Jwt.EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _12PC_Jwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserSongHistoryController : ControllerBase
    {
        private readonly IUserSongHistoryService _userSongHistoryService;

        public UserSongHistoryController(IUserSongHistoryService userSongHistoryService)
        {
            _userSongHistoryService = userSongHistoryService;
        }

        [HttpGet]
        public IActionResult GetList()
        {
            var userSongHistorys = _userSongHistoryService.GetAll();
            return Ok(userSongHistorys);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var userSongHistory = _userSongHistoryService.GetById(id);
            if (userSongHistory == null)
                return NotFound("Kullanıcı Şarkı Geçmişi bulunamadı!");

            return Ok(userSongHistory);
        }

        [HttpPost]
        public IActionResult CreateUserSongHistory(CreateUserSongHistoryDto createUserSongHistoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userSongHistory = new UserSongHistory
            {
                UserId = createUserSongHistoryDto.UserId,
                SongId = createUserSongHistoryDto.SongId,
                ListenCount = createUserSongHistoryDto.ListenCount,
                LastListened = createUserSongHistoryDto.LastListened               
            };

            _userSongHistoryService.Insert(userSongHistory);

            return Ok("Kullanıcı Şarkı Geçmişi başarıyla eklendi!");
        }

        [HttpPut]
        public IActionResult UpdateUserSongHistory(UpdateUserSongHistoryDto updateUserSongHistoryDto)
        {
            var existingUserSongHistory = _userSongHistoryService.GetById(updateUserSongHistoryDto.Id);
            if (existingUserSongHistory == null)
                return NotFound("Güncellenecek kullanıcı şarkı geçmişi bulunamadı!");

            existingUserSongHistory.UserId = updateUserSongHistoryDto.UserId;
            existingUserSongHistory.SongId = updateUserSongHistoryDto.SongId;
            existingUserSongHistory.ListenCount = updateUserSongHistoryDto.ListenCount;
            existingUserSongHistory.LastListened = updateUserSongHistoryDto.LastListened;

            _userSongHistoryService.Update(existingUserSongHistory);

            return Ok("Kullanıcı Şarkı Geçmişi başarıyla güncellendi!");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUserSongHistory(int id)
        {
            var userSongHistory = _userSongHistoryService.GetById(id);
            if (userSongHistory == null)
                return NotFound("Silinecek kullanıcı şarkı geçmişi bulunamadı!");

            _userSongHistoryService.Delete(userSongHistory);
            return Ok("Kullanıcı Şarkı Geçmişi başarıyla silindi!");
        }

        [HttpGet("GetUserSongHistoriesWithUserAndSong")]
        public IActionResult GetUserSongHistoriesWithUserAndSong()
        {
            var userSongHistorys = _userSongHistoryService.TGetUserSongHistoriesWithUserAndSong();

            var result = userSongHistorys.Select(u => new ResultUserSongHistoryDto
            {
                Id = u.Id,
                UserId = u.UserId,
                LastListened = u.LastListened,
                ListenCount = u.ListenCount,
                SongId = u.SongId,
                UserNameSurname = u.User.NameSurname,
                SongName = u.Song.Title,
                SongArtist = u.Song.Artist

            }).ToList();

            return Ok(result);
        }

        [HttpGet("GetPagedList")]
        public IActionResult GetPagedList(int page = 1, int pageSize = 10)
        {
            var query = _userSongHistoryService.TGetUserSongHistoriesWithUserAndSong();

            var totalCount = query.Count();

            var items = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ResultUserSongHistoryDto
                {
                    Id = x.Id,
                    SongName = x.Song.Title,
                    SongArtist = x.Song.Artist,
                    UserNameSurname = x.User.NameSurname,
                    ListenCount = x.ListenCount,
                    LastListened = x.LastListened,
                    SongId = x.SongId,
                    UserId = x.UserId
                })
                .ToList();

            var result = new PagedResultDto<ResultUserSongHistoryDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };

            return Ok(result);
        }

        [HttpGet("GetTotalListeningCount")]
        public IActionResult GetTotalListeningCount()
        {
            int listeningCount = _userSongHistoryService.TGetTotalListeningCount();
            return Ok(listeningCount);
        }

        [HttpGet("GetMostListenedSong")]
        public IActionResult GetMostListenedSong()
        {
            var song = _userSongHistoryService.TGetMostListenedSong();
            return Ok(song);
        }

        [HttpGet("GetMonthlyListeningStats")]
        public IActionResult GetMonthlyListeningStats()
        {
            var result = _userSongHistoryService.TGetMonthlyListeningStats();

            return Ok(result);
        }

        [HttpGet("GetTypeByAmount")]
        public IActionResult GetTypeByAmount()
        {            
            var histories = _userSongHistoryService.TGetAllWithSong();

            var result = histories
                .GroupBy(x => x.Song.Genre)
                .Select(g => new GenreCountDto
                {
                    Genre = g.Key,
                    Count = g.Sum(x => x.ListenCount)
                })
                .ToList();

            return Ok(result);
        }

        [HttpPost("IncrementListenCount/{songId}")]
        public async Task<IActionResult> IncrementListenCount(int songId, [FromQuery] int userId)
        {
            await _userSongHistoryService.TIncrementListenCountAsync(userId, songId);
            return Ok("Dinleme güncellendi.");
        }
    }
}
