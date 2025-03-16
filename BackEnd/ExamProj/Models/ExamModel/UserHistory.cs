using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Column(TypeName = "varchar(50)")]
        public string ExamTitle { get; set; }

        [Required]
        public double TotalScorePercentage { get; set; }      
        
        [Required]
        public double TotalScoreWeightPercentage { get; set; }

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
