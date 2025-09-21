using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.EntityLayer.Concrete
{
    public class Recommendation
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int SongId { get; set; }
        public Song Song { get; set; }

        public int Score { get; set; }
    }
}
