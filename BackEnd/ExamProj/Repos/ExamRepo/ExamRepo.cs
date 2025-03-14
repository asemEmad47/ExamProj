using ExamProj.Interfaces.ExamInterfaces;
using ExamProj.Models.ExamModel;
using ExamProj.Services.ExamServices;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.Interfaces;
using WebApplication1.Models.ExamModel;

namespace ExamProj.Repos.ExamRepo
{
    public class ExamRepo:IExamRepo
    {
        private readonly Context _context;
        private readonly IBaseRepo<Exam> _baseRepo;
        private readonly FieldsValidator _validator;
        public ExamRepo(Context context , IBaseRepo<Exam> baseRepo , FieldsValidator fieldsValidator)
        {
            _context = context;
            _baseRepo = baseRepo;
            _validator = fieldsValidator;
        }

        public async Task<(Exam?, string?)> AddNewExam(Exam exam)
        {
            var ValidatorResult = _validator.ValidateExamFields(exam);

            if (ValidatorResult.Item1)
            {
                var NewExam = await _baseRepo.Add(exam);
                if (NewExam == null)
                {
                    return (null,"Un expicted error happend!");
                }
                return (NewExam, "");

            }
            return (null,ValidatorResult.Item2);
        }

        public async Task<UserHistory?> CorrectUserAnswers(Exam exam , int UserId)
        {
            var OriginalExam = _context.exams
                .Include(q => q.Questions)
                .FirstOrDefault(e => e.UserId == UserId);

            if (OriginalExam == null)
            {
                return null;
            }
            int NumOfQuestions = OriginalExam.Questions.Count;
            double TotalScorePercentage = 0.0;
            int RightAnswers = 0;
            int WrongAnswers = 0;
            ExamStatus ExamStatus;

            foreach (var OrginalQuestion in OriginalExam.Questions)
            {
                foreach (var SolvedQuestion in exam.Questions)
                {
                    if (OrginalQuestion.QuestionId == SolvedQuestion.QuestionId) { 

                        if(OrginalQuestion.RightAnswerId == SolvedQuestion.RightAnswerId)
                        {
                            TotalScorePercentage += OrginalQuestion.QuestionWeight;
                            RightAnswers++;
                        }
                        else
                        {
                            WrongAnswers++;
                        }
                        exam.Questions.Remove(SolvedQuestion);
                        break;
                    }
                }
            }
            TotalScorePercentage = TotalScorePercentage / NumOfQuestions;

            if(TotalScorePercentage >= 0.6)
            {
                ExamStatus = ExamStatus.Pass;
            }
            else
            {
                ExamStatus = ExamStatus.Fail;

            }
            UserHistory userHistory = new UserHistory()
            {
                UserId = UserId,
                ExamId = exam.ExamId,
                TotalScore = TotalScorePercentage,
                NumOfCorrectAnswers = RightAnswers,
                NumOfWrongAnswers = WrongAnswers,
                ExamStatus = ExamStatus

            };
            return userHistory;
        }

        public async Task<bool> DeleteExam(int ExamId)
        {
            var Exam = await _baseRepo.Delete(ExamId);

            if (!Exam)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<(Exam?, string)> GetExam(int ExamId)
        {
            var Exam = await _baseRepo.GetByID(ExamId);

            if (Exam == null)
            {
                return (null,"There is no Exam with that ID!");
            }
            else
            {
                return (Exam,"");
            }
        }

        public async Task<(Exam?, string?)> UpdateExam(Exam updatedExam, int id)
        {
            try
            {
                var existingExam = await _context.exams
                    .Include(e => e.Questions)
                        .ThenInclude(q => q.Answers)  // Include all answers
                    .FirstOrDefaultAsync(e => e.ExamId == id);

                if (existingExam == null)
                    return (null, "Exam not found");

                // Update exam properties
                existingExam.TotalScore = updatedExam.TotalScore;
                existingExam.UserId = updatedExam.UserId;

                // Update questions and answers
                foreach (var updatedQuestion in updatedExam.Questions)
                {
                    var existingQuestion = existingExam.Questions
                        .FirstOrDefault(q => q.QuestionId == updatedQuestion.QuestionId);

                    if (existingQuestion != null)
                    {
                        // Update question properties
                        existingQuestion.QuestionTitle = updatedQuestion.QuestionTitle;
                        existingQuestion.QuestionWeight = updatedQuestion.QuestionWeight;
                        existingQuestion.RightAnswerId = updatedQuestion.RightAnswerId;

                        // Update existing answers only
                        foreach (var existingAnswer in existingQuestion.Answers)
                        {
                            var updatedAnswer = updatedQuestion.Answers
                                .FirstOrDefault(a => a.AnswerId == existingAnswer.AnswerId);

                            if (updatedAnswer != null)
                            {
                                // Update the existing answer text
                                existingAnswer.AnswerTxt = updatedAnswer.AnswerTxt;
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
                return (existingExam, "");
            }
            catch (Exception e)
            {
                return (null, e.Message);
            }
        }


    }
}
