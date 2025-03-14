using ExamProj.Repos.AuthRepos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApplication1.Models.UserModels;

namespace ExamProj.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly AuthRepo _authRepo;

        public AuthController(AuthRepo authRepo) {
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
        public async Task<IActionResult> AdminLogin(string Email, string Password)
        {
            var Result = await _authRepo.AdminLogin(Email, Password);
            if (Result == null)
            {
                return NotFound();
            }
            return Ok(Result);
        }

        [HttpPost("StudentLogin")]
        public async Task<IActionResult> StudentLogin(string Email, string Password)
        {
            var Result = await _authRepo.StudentLogin(Email, Password);

            if (Result == null )
            {
                return NotFound();
            }
            return Ok(Result);
        }
        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh(string ExpiredToken) {

            var Result = await _authRepo.Refresh(ExpiredToken);

            if (Result == null)
            {
                return Unauthorized();
            }
            return Ok(Result);
        }
    }
}
