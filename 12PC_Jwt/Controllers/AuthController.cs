using _12PC_Jwt.Models;
using Jwt.BusinessLayer.Abstract;
using Jwt.DtoLayer.UserDtos;
using Jwt.EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace _12PC_Jwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AuthController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterUserDto dto)
        {
            var existingUser = _userService.GetByEmail(dto.Email);
            if (existingUser != null) return BadRequest("Email already exists");

            CreatePasswordHash(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                PackageId = dto.PackageId,
                NameSurname = dto.NameSurname,
                City = dto.City
            };

            _userService.Insert(user);
            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public IActionResult Login(LoginUserDto dto)
        {
            var user = _userService.GetByEmail(dto.Email);
            if (user == null) return BadRequest("User not found");

            if (!VerifyPasswordHash(dto.Password, user.PasswordHash, user.PasswordSalt))
                return BadRequest("Wrong password");

            var token = CreateToken(user);
            return Ok(new { token });
        }

        #region Helper Methods
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(storedHash);
            }
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion


        //private readonly JWTTokenGenerator _jwtGenerator;

        //public AuthController()
        //{            
        //    _jwtGenerator = new JWTTokenGenerator();
        //}

        //private static readonly List<UserLogin> _users = new List<UserLogin>
        //{
        //    new UserLogin { Username = "murat", Password = "12345" },
        //    new UserLogin { Username = "ayse", Password = "abc123" },
        //    new UserLogin { Username = "mehmet", Password = "pass123" },
        //    new UserLogin { Username = "admin", Password = "admin123" },
        //    new UserLogin { Username = "guest", Password = "guest123" }
        //};

        //[HttpPost("login")]
        //public IActionResult Login(UserInfo userInfo)
        //{            
        //    var user = _users.FirstOrDefault(u =>
        //        u.Username == userInfo.Username && u.Password == userInfo.Password);

        //    if (user != null)
        //    {
        //        var token = _jwtGenerator.GenerateToken(userInfo);
        //        return Ok(token);
        //    }

        //    return Unauthorized("Hatalı kullanıcı adı veya şifre!");
        //}

        //[HttpPost("createToken")]
        //public IActionResult CreateToken([FromBody] UserInfo user)
        //{
        //    if (user == null)
        //    {
        //        return BadRequest("Kullanıcı bilgisi boş olamaz.");
        //    }

        //    var token = _jwtGenerator.GenerateToken(user);

        //    return Ok(new { Token = token });
        //}
    }
}
