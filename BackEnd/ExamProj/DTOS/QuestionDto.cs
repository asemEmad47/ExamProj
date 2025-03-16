using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Models.ExamModel;

namespace ExamProj.DTOS
{
    public class QuestionDto
    {
        public int QuestionId { get; set; }
        public double QuestionWeight { get; set; }
        public string QuestionTitle { get; set; }
        public List<AnswerDto> Answers { get; set; }
    }
}
