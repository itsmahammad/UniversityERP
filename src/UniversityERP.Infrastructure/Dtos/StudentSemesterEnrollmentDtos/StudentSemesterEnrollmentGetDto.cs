using UniversityERP.Domain.Enums;
using UniversityERP.Infrastructure.Dtos.EnrollmentCourseDtos;

namespace UniversityERP.Infrastructure.Dtos.StudentSemesterEnrollmentDtos;

public class StudentSemesterEnrollmentGetDto
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public string StudentFullName { get; set; } = default!;
    public Guid SemesterId { get; set; }
    public string SemesterName { get; set; } = default!;
    public Guid AcademicProgramId { get; set; }
    public string AcademicProgramName { get; set; } = default!;
    public StudentStatus StudentStatus { get; set; }
    public int MaxCredits { get; set; }
    public int TotalEnrolledCredits { get; set; }
    public decimal? StartingCgpa { get; set; }
    public StudentSemesterEnrollmentStatus Status { get; set; }
    public string? Notes { get; set; }
    public List<EnrollmentCourseGetDto> EnrollmentCourses { get; set; } = [];
}
