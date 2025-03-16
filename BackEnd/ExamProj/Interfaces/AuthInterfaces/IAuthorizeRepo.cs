using WebApplication1.Models.UserModels;
using ExamProj.Helpers;
namespace ExamProj.Interfaces.AuthInterfaces
{
    public interface IAuthorizeRepo
    {
        Task<TokenResponse?> StudentLogin(string Email , string Password);
        Task<TokenResponse?> AdminLogin(string Email , string Password);
        Task<string> AdminRegestration(User user);
        Task<string> StudntRegestration(User user);
        Task<TokenResponse> Refresh(RefreshTokenObj refreshTokenObj);
    }
}
