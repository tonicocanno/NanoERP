using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NanoERP.API.Domain.Entities;

namespace NanoERP.API.Utilities
{
    public class TokenService
    {
        public static string Generate(string key, User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = ClaimsIdentity(user),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private static ClaimsIdentity ClaimsIdentity(User user)
        {
            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, user.StringId),
                user.Email != null ? new (ClaimTypes.Email, user.Email) : new (
                    ClaimTypes.Name, user.Username)
            };

            return new ClaimsIdentity(claims);
        }
    }
}