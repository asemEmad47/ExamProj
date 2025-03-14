using ExamProj.Models.ExamModel;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.ExamModel;
using WebApplication1.Models.UserModels;

namespace WebApplication1.Context
{
    public class Context:DbContext
    {
        public DbSet<User> users { get; set; }

        public DbSet<Exam> exams { get; set; }
        public DbSet<Question> questions { get; set; }
        public DbSet<Answer> answers { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();

            modelBuilder.Entity<UserHistory>()
                .Property(u => u.ExamStatus)
                .HasConversion<string>();

            modelBuilder.Entity<UserHistory>()
                .HasKey(uh => new { uh.UserId, uh.HistoryId});


            modelBuilder.Entity<Question>()
                .HasMany(q => q.Answers)
                .WithOne()
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            var SuperAdmin = new User()
            {
                UserId = 1,
                UserName = "Asem emad",
                Password = "AQAAAAIAAYagAAAAEDlrL6MkkpZYVKNrABq3VrejgMfdxq9AMeR1iz55Dw4GwYdudj+jKaPpok/9G/Ld8g==",
                Email = "asememad984@gmail.com",
                Role = Role.SuperAdmin
            };

            modelBuilder.Entity<User>().HasData(SuperAdmin);
        }
    }
}
