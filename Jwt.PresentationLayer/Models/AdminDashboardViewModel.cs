using Jwt.PresentationLayer.Dtos.ArtistDtos;
using Jwt.PresentationLayer.Dtos.SongDtos;

namespace Jwt.PresentationLayer.Models
{
    public class AdminDashboardViewModel
    {
        public List<ResultSongDto> MostPopularSongs { get; set; }
        public List<ResultSongDto> AllSongs { get; set; }
        public int TotalUsers { get; set; }
        public int TotalSongs { get; set; }
        public int TotalListening { get; set; }
        public string MostListenedSong { get; set; }
        public List<ResultArtistDto> Artists { get; set; }
        public string Error { get; set; }
    }
}
