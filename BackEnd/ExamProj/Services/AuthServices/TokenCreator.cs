using ExamProj.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApplication1.Models.UserModels;

namespace ExamProj.Services.AuthServices
{
    public class TokenCreator
    {
        private readonly TokenConfig token;

        public TokenCreator(TokenConfig token)
        {
            this.token = token;
        }
        public string CreateToken(User user)
        {
            var TokenHandler = new JwtSecurityTokenHandler();
            var TokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = token.Issuer,
                Audience = token.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token.Key)), SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(
                [
                    new(ClaimTypes.NameIdentifier , user.UserId.ToString()),
                    new(ClaimTypes.Role , user.Role.ToString())
                ])
            };
            var SecurityToken = TokenHandler.CreateToken(TokenDescriptor);
            var AccessToken = TokenHandler.WriteToken(SecurityToken);
            return AccessToken;
        }
        public string CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
