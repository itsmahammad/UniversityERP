using AutoMapper;
using UniversityERP.Domain.Entities;
using UniversityERP.Infrastructure.Dtos.ProgramCourseDtos;

namespace UniversityERP.Infrastructure.Profiles;

public class ProgramCourseProfile : Profile
{
    public ProgramCourseProfile()
    {
        CreateMap<ProgramCourseCreateDto, ProgramCourse>();
        CreateMap<ProgramCourseUpdateDto, ProgramCourse>();

        CreateMap<ProgramCourse, ProgramCourseGetDto>()
            .ForMember(d => d.AcademicProgramName, o => o.MapFrom(s => s.AcademicProgram.Name))
            .ForMember(d => d.AcademicCourseCode, o => o.MapFrom(s => s.AcademicCourse.Code))
            .ForMember(d => d.AcademicCourseName, o => o.MapFrom(s => s.AcademicCourse.Name))
            .ForMember(d => d.EctsCredits, o => o.MapFrom(s => s.AcademicCourse.EctsCredits));
    }
}
