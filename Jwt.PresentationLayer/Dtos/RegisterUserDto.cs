using System.ComponentModel.DataAnnotations;

namespace Jwt.PresentationLayer.Dtos
{
    public class RegisterUserDto
    {
        public string NameSurname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string City { get; set; }     
        public int PackageId { get; set; }
    }
}
