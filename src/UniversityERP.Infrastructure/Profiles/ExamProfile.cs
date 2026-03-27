using AutoMapper;
using UniversityERP.Domain.Entities;
using UniversityERP.Infrastructure.Dtos.ExamDtos;

namespace UniversityERP.Infrastructure.Profiles;

public class ExamProfile : Profile
{
    public ExamProfile()
    {
        CreateMap<ExamCreateDto, Exam>();
        CreateMap<ExamUpdateDto, Exam>();

        CreateMap<Exam, ExamGetDto>()
            .ForMember(d => d.AcademicCourseCode, o => o.MapFrom(s => s.CourseOffering.AcademicCourse.Code))
            .ForMember(d => d.AcademicCourseName, o => o.MapFrom(s => s.CourseOffering.AcademicCourse.Name))
            .ForMember(d => d.SemesterName, o => o.MapFrom(s => s.CourseOffering.Semester.AcademicYear.Name + " " + s.CourseOffering.Semester.Term))
            .ForMember(d => d.TeacherFullName, o => o.MapFrom(s => s.CourseOffering.Teacher.User.FullName))
            .ForMember(d => d.Section, o => o.MapFrom(s => s.CourseOffering.Section));
    }
}
