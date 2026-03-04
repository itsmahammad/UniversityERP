using AutoMapper;
using UniversityERP.Domain.Entities;
using UniversityERP.Infrastructure.Dtos.AcademicCourseDtos;

namespace UniversityERP.Infrastructure.Profiles;

public class AcademicCourseProfile : Profile
{
    public AcademicCourseProfile()
    {
        CreateMap<AcademicCourseCreateDto, AcademicCourse>()
            .ForMember(d => d.Code, o => o.MapFrom(s => s.Code.Trim().ToUpperInvariant()))
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Name.Trim()))
            .ForMember(d => d.Description, o => o.MapFrom(s => string.IsNullOrWhiteSpace(s.Description) ? null : s.Description.Trim()));

        CreateMap<AcademicCourseUpdateDto, AcademicCourse>()
            .ForMember(d => d.Code, o => o.MapFrom(s => s.Code.Trim().ToUpperInvariant()))
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Name.Trim()))
            .ForMember(d => d.Description, o => o.MapFrom(s => string.IsNullOrWhiteSpace(s.Description) ? null : s.Description.Trim()));

        CreateMap<AcademicCourse, AcademicCourseGetDto>();
    }
}