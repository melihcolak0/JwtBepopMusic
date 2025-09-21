using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.BusinessLayer.ML.Models
{
    public class UserSongRating
    {
        public int UserId { get; set; }
        public int SongId { get; set; }
        public float Rating { get; set; }
    }
}
