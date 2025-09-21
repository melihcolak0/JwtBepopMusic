using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.DtoLayer.SongDtos
{
    public class UpdateSongDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Genre { get; set; }
        public string DataSourceUrl { get; set; }
        public string CoverImageUrl { get; set; }
        public string ArtistImageUrl { get; set; }

        public string Duration { get; set; }
        public DateTime ReleaseDate { get; set; }

        public int PackageId { get; set; }
    }
}
