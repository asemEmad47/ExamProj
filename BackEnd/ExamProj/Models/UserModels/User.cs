using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.UserModels
{
    [Table("User", Schema = "Users")]
    public class User
    {

        [Key]
        public int UserId { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Email { get; set; }

        [Required]
        [Column(TypeName = "varchar(25)")]
        public string UserName { get; set; }

        [Required]
        [Column(TypeName = "varchar(25)")]
        public string Password { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }
        public Role Role { get; set; }
    }
}
