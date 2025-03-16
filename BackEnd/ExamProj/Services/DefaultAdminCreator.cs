using Microsoft.AspNetCore.Identity;
using WebApplication1.Models.UserModels;

namespace ExamProj.Services
{
    public static class DefaultAdminCreator
    {
        public static User GetSuperAdminUser()
        {
            var passwordHasher = new PasswordHasher<User>();
            var superAdmin = new User()
            {
                UserName = "Asem emad",
                Email = "asememad984@gmail.com",
                Role = Role.SuperAdmin,
                RefreshToken = "xyz",
                RefreshTokenExpiryTime = DateTime.UtcNow,
                
            };

            // Hash the password before setting it
            var hashedPassword = passwordHasher.HashPassword(superAdmin, "Exam $Website_563");
            superAdmin.Password = hashedPassword;

            return superAdmin;
        }
    }
}
