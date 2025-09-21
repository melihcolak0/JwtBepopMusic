using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace _12PC_Jwt.Models
{
    public class JWTTokenGenerator
    {
        private readonly string _secretKey = "supersecretkey123456+-*/ABCDASPNETCOREAPI0011++--"; // Gerçekte appsettings.json içine koyulmalı
        private readonly string _issuer = "https://localhost";       // Token'ı üreten yer
        private readonly string _audience = "https://localhost";     // Token'ı kullanacak yer

        public string GenerateToken(UserInfo user)
        {
            // Payload içine eklenecek claimler
            var claims = new[]
            {
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim("Username", user.Username),
                new Claim("Password", user.Password),
                new Claim("City", user.City),
                new Claim("District", user.District),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // unique id
            };

            // Gizli anahtarı byte dizisine çevir
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

            // İmzalama kimliği
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Token oluşturma
            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30), // 30 dk geçerli
                signingCredentials: creds
            );

            // Token string olarak döndürülüyor
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
