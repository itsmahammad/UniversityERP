using AutoMapper;
using UniversityERP.Domain.Entities;
using UniversityERP.Infrastructure.Dtos.StudentSemesterEnrollmentDtos;

namespace UniversityERP.Infrastructure.Profiles;

public class StudentSemesterEnrollmentProfile : Profile
{
    public StudentSemesterEnrollmentProfile()
    {
        CreateMap<StudentSemesterEnrollmentCreateDto, StudentSemesterEnrollment>()
            .ForMember(d => d.Notes, o => o.MapFrom(s => string.IsNullOrWhiteSpace(s.Notes) ? null : s.Notes.Trim()));

        CreateMap<StudentSemesterEnrollment, StudentSemesterEnrollmentGetDto>()
            .ForMember(d => d.StudentFullName, o => o.MapFrom(s => s.Student.User.FullName))
            .ForMember(d => d.SemesterName, o => o.MapFrom(s => s.Semester.AcademicYear.Name + " " + s.Semester.Term))
            .ForMember(d => d.AcademicProgramName, o => o.MapFrom(s => s.AcademicProgram.Name));
    }
}
