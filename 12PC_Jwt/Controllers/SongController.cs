using Jwt.BusinessLayer.Abstract;
using Jwt.DtoLayer.SongDtos;
using Jwt.EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _12PC_Jwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongController : ControllerBase
    {
        private readonly ISongService _songService;

        public SongController(ISongService songService)
        {
            _songService = songService;
        }

        [HttpGet]
        public IActionResult GetList()
        {
            var songs = _songService.GetAll();
            return Ok(songs);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var song = _songService.GetById(id);
            if (song == null)
                return NotFound("Şarkı bulunamadı!");

            return Ok(song);
        }

        [HttpPost]
        public IActionResult CreateSong([FromBody] CreateSongDto createSongDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            TimeSpan durationValue;
            if (!TimeSpan.TryParse(createSongDto.Duration, out durationValue))
            {
                return BadRequest("Duration formatı hatalı. Örn: '00:04:10'");
            }

            var song = new Song
            {
                Title = createSongDto.Title,
                Album = createSongDto.Album,
                Artist = createSongDto.Artist,
                Genre = createSongDto.Genre,
                DataSourceUrl = createSongDto.DataSourceUrl,
                Duration = durationValue,
                PackageId= createSongDto.PackageId,
                ReleaseDate = createSongDto.ReleaseDate,
                CoverImageUrl = createSongDto.CoverImageUrl,
                ArtistImageUrl = createSongDto.ArtistImageUrl
            };

            _songService.Insert(song);

            return Ok("Şarkı başarıyla eklendi!");
        }

        [HttpPut]
        public IActionResult UpdateSong(UpdateSongDto updateSongDto)
        {
            var existingSong = _songService.GetById(updateSongDto.Id);
            if (existingSong == null)
                return NotFound("Güncellenecek şarkı bulunamadı!");

            TimeSpan durationValue;
            if (!TimeSpan.TryParse(updateSongDto.Duration, out durationValue))
            {
                return BadRequest("Duration formatı hatalı. Örn: '00:04:10'");
            }

            existingSong.Title = updateSongDto.Title;
            existingSong.Artist = updateSongDto.Artist;
            existingSong.Album = updateSongDto.Album;
            existingSong.Genre = updateSongDto.Genre;
            existingSong.DataSourceUrl = updateSongDto.DataSourceUrl;
            existingSong.Duration = durationValue;
            existingSong.PackageId = updateSongDto.PackageId;
            existingSong.ReleaseDate = updateSongDto.ReleaseDate;
            existingSong.CoverImageUrl = updateSongDto.CoverImageUrl;
            existingSong.ArtistImageUrl = updateSongDto.ArtistImageUrl;

            _songService.Update(existingSong);

            return Ok("Şarkı başarıyla güncellendi!");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSong(int id)
        {
            var song = _songService.GetById(id);
            if (song == null)
                return NotFound("Silinecek şarkı bulunamadı!");

            _songService.Delete(song);
            return Ok("Şarkı başarıyla silindi!");
        }

        [HttpGet("GetListByPackageLevel/{userPackageId}")]
        public IActionResult GetListByPackageLevel(int userPackageId)
        {
            var songs = _songService.TListSongsByPackageLevel(userPackageId);
            return Ok(songs);
        }

        [HttpGet("GetListLast5Songs/{userPackageId}")]
        public IActionResult GetListLast5Songs(int userPackageId)
        {
            var songs = _songService.TListLast5SongsByPackageLevel(userPackageId);
            return Ok(songs);
        }

        [HttpGet("GetList8MostPopularSongs/{userPackageId}")]
        public IActionResult GetList8MostPopularSongs(int userPackageId)
        {
            var songs = _songService.TList8MostPopularSongsByPackageLevel(userPackageId);
            return Ok(songs);
        }

        [HttpGet("GetListLast10SongsAWeekByPackageLevel/{userPackageId}")]
        public IActionResult GetListLast10SongsAWeekByPackageLevel(int userPackageId)
        {
            var songs = _songService.TListLast10SongsAWeekByPackageLevel(userPackageId);
            return Ok(songs);
        }

        [HttpGet("GetList5SongsByPackageLevelAndArtist/{userPackageId}/{artist}")]
        public IActionResult GetList5SongsByPackageLevelAndArtist(int userPackageId, string artist)
        {
            var songs = _songService.TList5SongsByPackageLevelAndArtist(userPackageId, artist);
            return Ok(songs);
        }

        [HttpGet("GetListSongsWithPackages")]
        public IActionResult GetListSongsWithPackages()
        {
            var songsWithPackages = _songService.TListSongsWithPackages();

            var result = songsWithPackages.Select(s => new ResultSongDto
            {
                Id = s.Id,
                Title = s.Title,
                Artist = s.Artist,
                Album = s.Album,
                Genre = s.Genre,
                Duration = s.Duration,
                PackageId = s.PackageId,
                PackageName = s.Package.Name,
                ArtistImageUrl = s.ArtistImageUrl,
                CoverImageUrl = s.CoverImageUrl,
                DataSourceUrl = s.DataSourceUrl,
                ReleaseDate = s.ReleaseDate

            }).ToList();

            return Ok(result);
        }

        [HttpGet("GetTotalSongCount")]
        public IActionResult GetTotalSongCount()
        {
            var songCount = _songService.TGetTotalSongCount();
            return Ok(songCount);
        }
    }
}
