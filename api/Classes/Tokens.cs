using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace api.Classes
{
    public class Tokens
    {
        public static string GenerateToken(LoginView user, IConfiguration configuration)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username), //User.Identity.Name
                    new Claim(ClaimTypes.Role, user.Role), //User.IsInRole
                    new Claim("nome", user.Nome)
                }),
                Expires = DateTime.UtcNow.AddHours(Convert.ToInt32(configuration["Jwt:TokenValidityInHours"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:SecretKey"])), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
