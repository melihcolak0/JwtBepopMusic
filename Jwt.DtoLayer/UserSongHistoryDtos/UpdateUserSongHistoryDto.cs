using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.DtoLayer.UserSongHistoryDtos
{
    public class UpdateUserSongHistoryDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SongId { get; set; }
        public int ListenCount { get; set; }
        public DateTime LastListened { get; set; }
    }
}
