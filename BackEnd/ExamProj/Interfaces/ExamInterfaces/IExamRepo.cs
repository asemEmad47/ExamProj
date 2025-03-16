using ExamProj.DTOS;
using ExamProj.Models.ExamModel;
using WebApplication1.Models.ExamModel;

namespace ExamProj.Interfaces.ExamInterfaces
{
    public interface IExamRepo
    {
        Task<(Exam? , string?)> AddNewExam(Exam exam);
        Task<(Exam?, string?)> UpdateExam(Exam exam, int ExamId);
        Task<(ExamDto?, string?)> GetExam(int id);
        Task<bool> DeleteExam(int id);
        Task<UserHistory?> CorrectUserAnswers(Exam exam );
        Task<List<UserHistory>?> GetUserExamsHistory();
        Task<List<ExamDto>?> GetAll();
    }
}
