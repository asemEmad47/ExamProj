using WebApplication1.Models.ExamModel;
using WebApplication1.Models.UserModels;

namespace ExamProj.Services.ExamServices
{
    public class FieldsValidator
    {
        public (bool , string) ValidateExamFields(Exam exam)
        {
            foreach (var question in exam.Questions)
            {
                if(question.QuestionTitle.Length > 255)
                {
                    return (false, "Question title should not exceed 255 character");
                }
                else if (question.QuestionTitle.Length ==0 || question.QuestionTitle ==null)
                {
                    return (false, "Question title is required");

                }
                else if (question.Answers == null)
                {
                    return (false, "Answers are required");
                }      

                
                foreach (var Answer in question.Answers)
                {
                    if (Answer.AnswerTxt.Length > 255)
                    {
                        return (false, "Answer text should not exceed 255 character");
                    }

                    else if (Answer.AnswerTxt.Length == 0 || Answer.AnswerTxt == null)
                    {
                        return (false, "Answer text is required");
                    }
                }

            }
            return (true, "");
        }

        public (bool , string) ValidateUserFields(User user)
        {
            if (user.UserName.Length > 25)
            {
                return (false, "UserName should not exceed 255 character");
            }
            else if (user.UserName.Length == 0 || user.UserName == null)
            {
                return (false, "UserName title is required");

            }  
            
            if (user.Password.Length > 25)
            {
                return (false, "Password should not exceed 25 character");
            }
            else if (user.Password.Length == 0 || user.Password == null)
            {
                return (false, "Password title is required");

            }      
            
            if (user.Email.Length > 50)
            {
                return (false, "Email should not exceed 50 character");
            }
            else if (user.Email.Length == 0 || user.Email == null)
            {
                return (false, "Email title is required");

            }
            return (true, "");
        }
    }
}
