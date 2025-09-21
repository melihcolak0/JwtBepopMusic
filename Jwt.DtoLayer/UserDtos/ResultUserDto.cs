using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.DtoLayer.UserDtos
{
    public class ResultUserDto
    {
        public int Id { get; set; }
        public string NameSurname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public string Role { get; set; }
    }
}
