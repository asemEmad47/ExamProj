using AutoMapper;
using WebApplication1.Models.ExamModel;
namespace ExamProj.DTOS
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<Exam, ExamDto>();
            CreateMap<Exam, ExamDto>().ReverseMap(); 
            
            CreateMap<Question, QuestionDto>();
            CreateMap<Question, QuestionDto>().ReverseMap();       
            
            CreateMap<Answer, AnswerDto>();
            CreateMap<Answer, AnswerDto>().ReverseMap();
        }
    }
}
