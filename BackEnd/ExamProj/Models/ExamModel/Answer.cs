using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApplication1.Models.ExamModel
{
    public class Answer
    {
        [Key]
        public int AnswerId { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string AnswerTxt { get; set; }

        public int QuestionId { get; set; }

        [JsonIgnore]
        public  Question? Question { get; set; }
    }
}
