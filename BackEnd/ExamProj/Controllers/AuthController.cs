using ExamProj.Helpers;
using ExamProj.Interfaces.AuthInterfaces;
using ExamProj.Repos.AuthRepos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApplication1.Models.UserModels;

namespace ExamProj.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthorizeRepo _authRepo;

        public AuthController(IAuthorizeRepo authRepo) {
            _authRepo = authRepo;
        }


        [Authorize(Roles ="SuperAdmin")]
        [HttpPost("AdminRegestration")]
        public async Task<IActionResult> AdminRegestration(User admin)
        {
            var Result = await _authRepo.AdminRegestration(admin);

            if (string.IsNullOrEmpty(Result))
            {
                return Ok();
            }
            return NotFound(Result);
        }

        [HttpPost("StudentRegestration")]
        public async Task<IActionResult> StudntRegestration(User student)
        {
            var Result = await _authRepo.StudntRegestration(student);

            if (string.IsNullOrEmpty(Result))
            {
                return Ok();
            }
            return NotFound(Result);
        }


        [HttpPost("AdminLogin")]
        public async Task<IActionResult> AdminLogin([FromBody] LoginObj loginObj)
        {
            var Result = await _authRepo.AdminLogin(loginObj.Email, loginObj.Password);
            if (Result == null)
            {
                return NotFound();
            }
            return Ok(Result);
        }

        [HttpPost("StudentLogin")]
        public async Task<IActionResult> StudentLogin([FromBody] LoginObj loginObj)
        {
            Console.WriteLine("here");
            var Result = await _authRepo.StudentLogin(loginObj.Email, loginObj.Password);

            if (Result == null )
            {
                return NotFound();
            }
            return Ok(Result);
        }
        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenObj refreshTokenObj) {

            var Result = await _authRepo.Refresh(refreshTokenObj);

            if (Result == null)
            {
                return Unauthorized();
            }
            return Ok(Result);
        }
    }
}
