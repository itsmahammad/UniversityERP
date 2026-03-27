using UniversityERP.Domain.Enums;

namespace UniversityERP.Infrastructure.Dtos.EnrollmentCourseDtos;

public class EnrollmentCourseGetDto
{
    public Guid Id { get; set; }
    public Guid StudentSemesterEnrollmentId { get; set; }
    public Guid CourseOfferingId { get; set; }
    public Guid AcademicCourseId { get; set; }
    public string AcademicCourseCode { get; set; } = default!;
    public string AcademicCourseName { get; set; } = default!;
    public string TeacherFullName { get; set; } = default!;
    public string Section { get; set; } = string.Empty;
    public int AttemptNumber { get; set; }
    public int CreditsSnapshot { get; set; }
    public EnrollmentCourseStatus Status { get; set; }
    public DateTime EnrolledAt { get; set; }
    public DateTime? DroppedAt { get; set; }
}
