using AutoMapper;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Domain.Entities;
using UniversityERP.Infrastructure.Dtos.CourseOfferingDtos;

namespace UniversityERP.Infrastructure.Profiles;

public class CourseOfferingProfile : Profile
{
    public CourseOfferingProfile()
    {
        CreateMap<CourseOfferingCreateDto, CourseOffering>()
            .ForMember(d => d.Section, o => o.MapFrom(s => CourseOfferingSectionNormalizer.Normalize(s.Section)));

        CreateMap<CourseOfferingUpdateDto, CourseOffering>()
            .ForMember(d => d.Section, o => o.MapFrom(s => CourseOfferingSectionNormalizer.Normalize(s.Section)));

        CreateMap<CourseOffering, CourseOfferingGetDto>()
            .ForMember(d => d.AcademicCourseCode, o => o.MapFrom(s => s.AcademicCourse.Code))
            .ForMember(d => d.AcademicCourseName, o => o.MapFrom(s => s.AcademicCourse.Name))
            .ForMember(d => d.EctsCredits, o => o.MapFrom(s => s.AcademicCourse.EctsCredits))
            .ForMember(d => d.SemesterName, o => o.MapFrom(s => s.Semester.AcademicYear.Name + " " + s.Semester.Term))
            .ForMember(d => d.TeacherFullName, o => o.MapFrom(s => s.Teacher.User.FullName));
    }
}
