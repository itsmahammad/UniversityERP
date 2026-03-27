using AutoMapper;
using UniversityERP.Domain.Entities;
using UniversityERP.Infrastructure.Dtos.AttendanceRecordDtos;
using UniversityERP.Infrastructure.Dtos.AttendanceSessionDtos;

namespace UniversityERP.Infrastructure.Profiles;

public class AttendanceProfile : Profile
{
    public AttendanceProfile()
    {
        CreateMap<AttendanceSessionCreateDto, AttendanceSession>()
            .ForMember(d => d.Topic, o => o.MapFrom(s => string.IsNullOrWhiteSpace(s.Topic) ? null : s.Topic.Trim()));

        CreateMap<AttendanceSessionUpdateDto, AttendanceSession>()
            .ForMember(d => d.Topic, o => o.MapFrom(s => string.IsNullOrWhiteSpace(s.Topic) ? null : s.Topic.Trim()));

        CreateMap<AttendanceRecordCreateDto, AttendanceRecord>()
            .ForMember(d => d.Note, o => o.MapFrom(s => string.IsNullOrWhiteSpace(s.Note) ? null : s.Note.Trim()));

        CreateMap<AttendanceRecordUpdateDto, AttendanceRecord>()
            .ForMember(d => d.Note, o => o.MapFrom(s => string.IsNullOrWhiteSpace(s.Note) ? null : s.Note.Trim()));

        CreateMap<AttendanceRecord, AttendanceRecordGetDto>()
            .ForMember(d => d.StudentId, o => o.MapFrom(s => s.EnrollmentCourse.StudentSemesterEnrollment.StudentId))
            .ForMember(d => d.StudentFullName, o => o.MapFrom(s => s.EnrollmentCourse.StudentSemesterEnrollment.Student.User.FullName));

        CreateMap<AttendanceSession, AttendanceSessionGetDto>()
            .ForMember(d => d.AcademicCourseCode, o => o.MapFrom(s => s.CourseOffering.AcademicCourse.Code))
            .ForMember(d => d.AcademicCourseName, o => o.MapFrom(s => s.CourseOffering.AcademicCourse.Name))
            .ForMember(d => d.TeacherFullName, o => o.MapFrom(s => s.CourseOffering.Teacher.User.FullName))
            .ForMember(d => d.Section, o => o.MapFrom(s => s.CourseOffering.Section));
    }
}
