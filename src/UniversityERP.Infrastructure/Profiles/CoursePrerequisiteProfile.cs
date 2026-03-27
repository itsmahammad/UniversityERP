using AutoMapper;
using UniversityERP.Domain.Entities;
using UniversityERP.Infrastructure.Dtos.CoursePrerequisiteDtos;

namespace UniversityERP.Infrastructure.Profiles;

public class CoursePrerequisiteProfile : Profile
{
    public CoursePrerequisiteProfile()
    {
        CreateMap<CoursePrerequisiteCreateDto, CoursePrerequisite>();
        CreateMap<CoursePrerequisiteUpdateDto, CoursePrerequisite>();

        CreateMap<CoursePrerequisite, CoursePrerequisiteGetDto>()
            .ForMember(d => d.AcademicCourseCode, o => o.MapFrom(s => s.AcademicCourse.Code))
            .ForMember(d => d.AcademicCourseName, o => o.MapFrom(s => s.AcademicCourse.Name))
            .ForMember(d => d.PrerequisiteAcademicCourseCode, o => o.MapFrom(s => s.PrerequisiteAcademicCourse.Code))
            .ForMember(d => d.PrerequisiteAcademicCourseName, o => o.MapFrom(s => s.PrerequisiteAcademicCourse.Name));
    }
}
