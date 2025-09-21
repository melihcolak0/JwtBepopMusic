using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.EntityLayer.Concrete
{
    public class Playlist
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public int UserId { get; set; }
        public User User { get; set; }

        // Navigation
        public ICollection<PlaylistSong> PlaylistSongs { get; set; }
    }
}
