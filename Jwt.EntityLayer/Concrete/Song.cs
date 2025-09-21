using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.EntityLayer.Concrete
{
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public string Album { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string DataSourceUrl { get; set; } = string.Empty;
        public string CoverImageUrl { get; set; } = string.Empty;
        public string ArtistImageUrl { get; set; } = string.Empty;

        public TimeSpan Duration { get; set; }
        public DateTime ReleaseDate { get; set; }

        public int PackageId { get; set; }
        public Package Package { get; set; }

        public ICollection<UserSongHistory> ListeningHistory { get; set; }
        public ICollection<PlaylistSong> PlaylistSongs { get; set; }
        public ICollection<Recommendation> Recommendations { get; set; }
    }
}
