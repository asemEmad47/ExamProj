using Azure.Core;
using ExamProj.Helpers;
using ExamProj.Interfaces.AuthInterfaces;
using ExamProj.Services.AuthServices;
using ExamProj.Services.ExamServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using WebApplication1.Context;
using WebApplication1.Models.UserModels;

namespace ExamProj.Repos.AuthRepos
{
    public class AuthRepo : IAuthorizeRepo
    {
        private readonly FieldsValidator _validator;
        private readonly Context _context;
        private readonly TokenCreator _tokenCreator;

        public AuthRepo(FieldsValidator validator , Context context , TokenCreator tokenCreator)
        {
            _validator = validator;
            _context = context;
            _tokenCreator = tokenCreator;
        }
        public async Task<TokenResponse> AdminLogin(string Email, string Password)
        {
            return await Login(Email, Password , Role.SuperAdmin);
        }

        public async Task<string> AdminRegestration(User admin)
        {
            return await Register(admin, Role.SuperAdmin);
        }

        public async Task<TokenResponse> StudentLogin(string Email, string Password)
        {
            return await Login(Email, Password, Role.Student);
        }

        public async Task<string> StudntRegestration(User student)
        {
            return await Register(student, Role.Student);
        }

        private async Task<string> Register(User user , Role role)
        {
            var ValidatorResult = _validator.ValidateUserFields(user);

            if (ValidatorResult.Item1)
            {
                user.Role = role;

                var passwordHasher = new PasswordHasher<User>();

                user.Password = passwordHasher.HashPassword(user, user.Password);

                user.RefreshToken = _tokenCreator.CreateRefreshToken();
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                try
                {
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    return "";
                }
                catch (Exception e)
                {

                    return e.Message;
                }

            }
            return ValidatorResult.Item2;
        }

        private async Task<TokenResponse?> Login(string Email, string Password , Role role)
        {

            var user = await _context.users.Where(u => u.Role == role && Email.Equals(u.Email)).FirstOrDefaultAsync();

            if (user == null) {
                return null;
            }
            var passwordHasher = new PasswordHasher<User>();
            Console.WriteLine("here2 "+ user==null +" "+user.UserName +" "+user.Role);

            var result = passwordHasher.VerifyHashedPassword(user, user.Password, Password);

            if (result == PasswordVerificationResult.Success)
            {
                string AccessToken = _tokenCreator.CreateToken(user);
                string RefreshToken = _tokenCreator.CreateRefreshToken();
                TokenResponse Response = new TokenResponse(AccessToken, RefreshToken);
                user.RefreshToken = RefreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                await _context.SaveChangesAsync();
                return Response;
            }
            return null;
        }

        public async Task<TokenResponse> Refresh(RefreshTokenObj refreshTokenObj)
        {
            var user = _context.users.FirstOrDefault(u => u.RefreshToken == refreshTokenObj.RefreshToken);

            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }

            var NewAccessToken = _tokenCreator.CreateToken(user);
            var NewRefreshToken = _tokenCreator.CreateRefreshToken();
            user.RefreshToken = NewRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); 
            await _context.SaveChangesAsync();
            TokenResponse Response = new TokenResponse(NewAccessToken, NewRefreshToken);

            return Response;
        }

    }
}
