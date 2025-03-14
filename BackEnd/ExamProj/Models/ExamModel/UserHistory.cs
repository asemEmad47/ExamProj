using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebApplication1.Models.UserModels;

namespace ExamProj.Models.ExamModel
{
    public class UserHistory
    {
        public int HistoryId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ExamId { get; set; }

        [Required]
        public double TotalScore { get; set; }

        [Required]
        public ExamStatus ExamStatus { get; set; }

        [Required]
        public int NumOfCorrectAnswers { get; set; }

        [Required]
        public int NumOfWrongAnswers { get; set; }

        [JsonIgnore]
        public User user { get; set; }
    }
}
