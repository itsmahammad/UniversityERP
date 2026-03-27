using AutoMapper;
using UniversityERP.Domain.Entities;
using UniversityERP.Infrastructure.Dtos.ExamResultDtos;

namespace UniversityERP.Infrastructure.Profiles;

public class ExamResultProfile : Profile
{
    public ExamResultProfile()
    {
        CreateMap<ExamResultCreateDto, ExamResult>();
        CreateMap<ExamResultUpdateDto, ExamResult>();

        CreateMap<ExamResult, ExamResultGetDto>()
            .ForMember(d => d.ExamType, o => o.MapFrom(s => s.Exam.ExamType))
            .ForMember(d => d.ExamDate, o => o.MapFrom(s => s.Exam.ExamDate))
            .ForMember(d => d.MaxScore, o => o.MapFrom(s => s.Exam.MaxScore))
            .ForMember(d => d.WeightPercentage, o => o.MapFrom(s => s.Exam.WeightPercentage))
            .ForMember(d => d.WeightedContribution, o => o.MapFrom(s => s.Exam.MaxScore == 0 ? 0 : (s.NumericScore / s.Exam.MaxScore) * s.Exam.WeightPercentage));
    }
}
