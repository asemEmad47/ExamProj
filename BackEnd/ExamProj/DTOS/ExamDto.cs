using WebApplication1.Models.ExamModel;

namespace ExamProj.DTOS
{
    public class ExamDto
    {
        public int ExamId { get; set; }
        public string ExamTitle { get; set; }
        public double TotalScore { get; set; }
        public List<QuestionDto> Questions { get; set; }
    }
}
