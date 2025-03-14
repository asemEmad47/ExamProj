using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.ExamModel
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }

        public int ExamId { get; set; }

        [Required]
        public int RightAnswerId { get; set; }  

        [Required]
        public double QuestionWeight { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string QuestionTitle { get; set; }

        [Required]
        public List<Answer> Answers { get; set; }
    }
}
