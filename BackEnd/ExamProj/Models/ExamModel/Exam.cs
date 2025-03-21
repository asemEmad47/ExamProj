﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WebApplication1.Models.UserModels;

namespace WebApplication1.Models.ExamModel
{
    public class Exam
    {
        [Key]
        public int ExamId { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string ExamTitle { get; set; }
        public double TotalScore { get; set; }

        [Required]
        [Column("AddedBy")]
        public int UserId { get; set; }

        [Required]
        public List<Question> Questions { get; set; }

        public User? user { get; set; }
    }
}
