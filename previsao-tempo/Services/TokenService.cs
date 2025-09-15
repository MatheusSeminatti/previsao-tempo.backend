using Microsoft.IdentityModel.Tokens;
using previsao_tempo.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace previsao_tempo.Services
{
    public static class TokenService
    {
        private static string? _secret = null;
        public static string? Secret
        {
            get
            {
                return _secret;
            }
            set
            {
                if (_secret == null)
                {
                    _secret = value;
                }
            }
        }

        public static string Generate(ApplicationUser user)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret!);
            var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GenerateClaims(user),
                Expires = DateTime.UtcNow.AddHours(5),
                SigningCredentials = credentials,
            };
            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }

        private static ClaimsIdentity GenerateClaims(ApplicationUser user)
        {
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email!));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.UserName!));

            return claimsIdentity;
        }
    }
}
