using AutoMapper;
using ExamProj.DTOS;
using ExamProj.Interfaces.ExamInterfaces;
using ExamProj.Models.ExamModel;
using ExamProj.Services.ExamServices;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication1.Context;
using WebApplication1.Models.ExamModel;

namespace ExamProj.Repos.ExamRepo
{
    public class ExamRepo:IExamRepo
    {
        private readonly Context _context;
        private readonly FieldsValidator _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public ExamRepo(Context context  , FieldsValidator fieldsValidator , IHttpContextAccessor httpContextAccessor , IMapper mapper)
        {
            _context = context;
            _validator = fieldsValidator;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<(Exam?, string?)> AddNewExam(Exam exam)
        {
            var ValidatorResult = _validator.ValidateExamFields(exam);


            if (ValidatorResult.Item1)
            {
                double TotalWeight = 0;

                foreach (var question in exam.Questions)
                {
                    TotalWeight += question.QuestionWeight;
                }

                var userId = int.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var user = _context.users.Find(userId);

                exam.user = user;
                exam.UserId = userId;
                exam.TotalScore = TotalWeight;
                await _context.exams.AddAsync(exam);


                try
                {
                    await _context.SaveChangesAsync();
                    return (exam, ""); 
                }
                catch (Exception ex)
                {
                    return (null, $"An error occurred while saving the exam: {ex.Message}");
                }
            }

            return (null, ValidatorResult.Item2);
        }


        public async Task<UserHistory?> CorrectUserAnswers(Exam exam )
        {
            var OriginalExam = _context.exams
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                .FirstOrDefault(e => e.ExamId== exam.ExamId);

            if (OriginalExam == null)
            {
                return null;
            }
            int NumOfQuestions = OriginalExam.Questions.Count;
            double TotalScorePercentage = 0.0;
            double TotalScoreWeightPercentage = 0.0;
            int RightAnswers = 0;
            int WrongAnswers = 0;
            ExamStatus ExamStatus;

            foreach (var OrginalQuestion in OriginalExam.Questions)
            {
                foreach (var SolvedQuestion in exam.Questions)
                {
                    if (OrginalQuestion.QuestionId == SolvedQuestion.QuestionId) {

                        if (OrginalQuestion.Answers[0].AnswerId == SolvedQuestion.Answers[0].AnswerId)
                        {
                            TotalScoreWeightPercentage += OrginalQuestion.QuestionWeight;
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
            TotalScorePercentage = (double)RightAnswers /(RightAnswers+WrongAnswers);
            TotalScoreWeightPercentage = (double)TotalScoreWeightPercentage / OriginalExam.TotalScore;
            if(TotalScorePercentage >= 0.6)
            {
                ExamStatus = ExamStatus.Pass;
            }
            else
            {
                ExamStatus = ExamStatus.Fail;

            }
            var UserId = int.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = _context.users.Find(UserId);

            UserHistory userHistory = new UserHistory()
            {
                UserId = UserId,
                ExamId = exam.ExamId,
                TotalScoreWeightPercentage = TotalScoreWeightPercentage,
                TotalScorePercentage = TotalScorePercentage,
                NumOfCorrectAnswers = RightAnswers,
                NumOfWrongAnswers = WrongAnswers,
                ExamStatus = ExamStatus,
                ExamTitle = OriginalExam.ExamTitle
            };
            _context.histories.Add(userHistory);
            await _context.SaveChangesAsync();
            return userHistory;
        }

        public async Task<bool> DeleteExam(int examId)
        {
            var exam = await _context.exams.FindAsync(examId); 

            if (exam == null)
            {
                return false;
            }

            _context.exams.Remove(exam);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<(ExamDto?, string)> GetExam(int ExamId)
        {
            var Exam = await _context.exams
                .Where(e => e.ExamId == ExamId)
                .Include(e => e.Questions)
                    .ThenInclude(a => a.Answers)
                    .FirstOrDefaultAsync();

            var ExamDto = _mapper.Map<ExamDto>(Exam);
            if (ExamDto == null)
            {
                return (null,"There is no Exam with that ID!");
            }
            else
            {
                return (ExamDto, "");
            }
        }

        public async Task<(Exam?, string?)> UpdateExam(Exam updatedExam, int id)
        {
            try
            {
                var existingExam = await _context.exams
                    .Include(e => e.Questions)
                        .ThenInclude(q => q.Answers)  
                    .FirstOrDefaultAsync(e => e.ExamId == id);

                if (existingExam == null)
                    return (null, "Exam not found");


                existingExam.ExamTitle = updatedExam.ExamTitle;

                foreach (var updatedQuestion in updatedExam.Questions)
                {
                    var existingQuestion = existingExam.Questions
                        .FirstOrDefault(q => q.QuestionId == updatedQuestion.QuestionId);

                    if (existingQuestion != null)
                    {
                        existingQuestion.QuestionTitle = updatedQuestion.QuestionTitle;
                        existingQuestion.QuestionWeight = updatedQuestion.QuestionWeight;

                        foreach (var UpdatedAnswer in updatedQuestion.Answers)
                        {
                            var existingAnswer = existingQuestion.Answers
                                .FirstOrDefault(q => q.AnswerId == UpdatedAnswer.AnswerId);

                            if (existingAnswer != null)
                            {

                                existingAnswer.AnswerTxt = UpdatedAnswer.AnswerTxt;

                            }
                        }

                    }

   
                }

                double TotalWeight = 0;
                foreach (var Question in updatedExam.Questions)
                {
                    TotalWeight += Question.QuestionWeight;
                }

                var userId = int.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var user = _context.users.Find(userId);

                existingExam.user = user;
                existingExam.UserId = userId;
                existingExam.TotalScore = TotalWeight;

                await _context.SaveChangesAsync();
                return (existingExam, "");
            }
            catch (Exception e)
            {
                return (null, e.Message);
            }
        }

        public async Task<List<ExamDto>?> GetAll()
        {
            var Exams = await _context.exams.AsNoTracking()
                .Include(q => q.Questions)
                    .ThenInclude(a =>a.Answers)
                .ToListAsync();
            var ExamDtos = _mapper.Map<List<ExamDto>>(Exams);
            return ExamDtos;
        }

        public async Task<List<UserHistory>?> GetUserExamsHistory()
        {

            var UserId = int.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var UserExamHistory = await _context.histories.Where(q => q.UserId == UserId).ToListAsync();
            return UserExamHistory;
        }
    }
}
