using AutoMapper;
using UniversityERP.Domain.Entities;
using UniversityERP.Infrastructure.Dtos.EnrollmentCourseDtos;

namespace UniversityERP.Infrastructure.Profiles;

public class EnrollmentCourseProfile : Profile
{
    public EnrollmentCourseProfile()
    {
        CreateMap<EnrollmentCourseCreateDto, EnrollmentCourse>();

        CreateMap<EnrollmentCourse, EnrollmentCourseGetDto>()
            .ForMember(d => d.AcademicCourseId, o => o.MapFrom(s => s.CourseOffering.AcademicCourseId))
            .ForMember(d => d.AcademicCourseCode, o => o.MapFrom(s => s.CourseOffering.AcademicCourse.Code))
            .ForMember(d => d.AcademicCourseName, o => o.MapFrom(s => s.CourseOffering.AcademicCourse.Name))
            .ForMember(d => d.TeacherFullName, o => o.MapFrom(s => s.CourseOffering.Teacher.User.FullName))
            .ForMember(d => d.Section, o => o.MapFrom(s => s.CourseOffering.Section));
    }
}
