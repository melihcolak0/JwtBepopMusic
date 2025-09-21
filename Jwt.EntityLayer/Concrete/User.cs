using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.EntityLayer.Concrete
{
    public class User
    {
        public int Id { get; set; }
        public string NameSurname { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Role { get; set; } = "User";

        public int PackageId { get; set; }
        public Package Package { get; set; }        
        
        public ICollection<UserSongHistory> ListeningHistory { get; set; }
        public ICollection<Playlist> Playlists { get; set; }
        public ICollection<Recommendation> Recommendations { get; set; }
    }
}
