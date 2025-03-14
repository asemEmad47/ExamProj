using ExamProj.Interfaces.ExamInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models.ExamModel;

namespace ExamProj.Controllers.ExamPackage
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamController : Controller
    {
        private readonly IExamRepo _examRepo;
        public ExamController(IExamRepo examRepo)
        {
            this._examRepo = examRepo;
        }

        [HttpPost("AddNewExam")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> AddNewExam([FromBody] Exam exam)
        {
            var result =await _examRepo.AddNewExam(exam);
            if (result.Item1 != null)
            {
                return Ok(result.Item1);
            }
            return BadRequest(result.Item2);
        }


        [HttpPut("UpdateExam")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> UpdateExam([FromBody] Exam exam , int ExamId)
        {
            var result = await _examRepo.UpdateExam(exam, ExamId);
            if (result.Item1 != null)
            {
                return Ok(result.Item1);
            }
            return BadRequest(result.Item2);
        }

        [HttpPut("GetExam")]
        [Authorize]

        public async Task<IActionResult> GetExam(int ExamId)
        {
            var result = await _examRepo.GetExam(ExamId);
            if (result.Item1 != null)
            {
                return Ok(result.Item1);
            }
            return BadRequest(result.Item2);
        }       
        
        [HttpDelete("DeleteExam")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteExam(int ExamId)
        {
            var result = await _examRepo.DeleteExam(ExamId);
            if (!result)
            {
                return NotFound("There is no exam with this ID!");
            }
            return Ok(true);
        }
    }
}
