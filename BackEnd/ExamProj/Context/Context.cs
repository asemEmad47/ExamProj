using ExamProj.Models.ExamModel;
using ExamProj.Services;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.ExamModel;
using WebApplication1.Models.UserModels;

namespace WebApplication1.Context
{
    public class Context : DbContext
    {
        public DbSet<User> users { get; set; }
        public DbSet<Exam> exams { get; set; }
        public DbSet<Question> questions { get; set; }
        public DbSet<Answer> answers { get; set; }
        public DbSet<UserHistory> histories { get; set; }


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
                .HasKey(uh => new { uh.HistoryId });

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Exam>()
                .HasMany(e => e.Questions)
                .WithOne(q => q.Exam)
                .OnDelete(DeleteBehavior.Cascade);  

            modelBuilder.Entity<Question>()
                .HasMany(q => q.Answers)
                .WithOne(a => a.Question)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
