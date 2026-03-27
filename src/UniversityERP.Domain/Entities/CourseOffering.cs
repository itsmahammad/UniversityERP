using UniversityERP.Domain.Entities.Common;

namespace UniversityERP.Domain.Entities;

public class CourseOffering : BaseAuditableEntity
{
    public Guid AcademicCourseId { get; set; }
    public AcademicCourse AcademicCourse { get; set; } = default!;

    public Guid SemesterId { get; set; }
    public Semester Semester { get; set; } = default!;

    public Guid TeacherId { get; set; }
    public Teacher Teacher { get; set; } = default!;

    public string Section { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public ICollection<EnrollmentCourse> EnrollmentCourses { get; set; } = new List<EnrollmentCourse>();
    public ICollection<Exam> Exams { get; set; } = new List<Exam>();
    public ICollection<AttendanceSession> AttendanceSessions { get; set; } = new List<AttendanceSession>();
}
