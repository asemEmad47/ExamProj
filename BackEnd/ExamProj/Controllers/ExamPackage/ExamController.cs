using ExamProj.Interfaces.ExamInterfaces;
using ExamProj.Models.ExamModel;
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
        [HttpGet("GetAllExams")]
        public async Task<IActionResult> GetAllExams()
        {
            var AllExams = await _examRepo.GetAll();

            if (AllExams == null)
            {
                return NotFound("There is no exams in the system yet");
            }

            return Ok(AllExams);
        }

        [HttpPut("UpdateExam/{ExamId}")]
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

        [HttpGet("GetExam/{ExamId}")]
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
        
        [HttpDelete("DeleteExam/{ExamId}")]
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

        [HttpPost("CorrectUserAnswers")]
        [Authorize]
        public async Task<IActionResult> CorrectUserAnswers(Exam exam)
        {
            var UserHistory = await _examRepo.CorrectUserAnswers(exam);
            if(UserHistory == null)
            {
                return BadRequest("Unexpicted error happend");
            }
            var result = new
            {
                ExamId = UserHistory.ExamId,
                TotalScore = UserHistory.TotalScorePercentage,
                TotalWeightedSccore = UserHistory.TotalScoreWeightPercentage,
                NumOfCorrectAnswers = UserHistory.NumOfCorrectAnswers,
                NumOfWrongAnswers = UserHistory.NumOfWrongAnswers,
                ExamStatus = UserHistory.ExamStatus.ToString()  ,
                ExamTitle = UserHistory.ExamTitle,
            };
            return Ok(result);
        }

        [HttpGet("GetUserExams")]
        [Authorize]
        public async Task<IActionResult> GetExams()
        {
            var UserExams = await _examRepo.GetUserExamsHistory();
            if (UserExams == null)
            {
                return BadRequest("This user doesn't had an exam yet!");
            }

            return Ok(UserExams);
        }
    }
}
