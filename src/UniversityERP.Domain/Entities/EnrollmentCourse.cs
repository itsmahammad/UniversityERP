using UniversityERP.Domain.Entities.Common;
using UniversityERP.Domain.Enums;

namespace UniversityERP.Domain.Entities;

public class EnrollmentCourse : BaseAuditableEntity
{
    public Guid StudentSemesterEnrollmentId { get; set; }
    public StudentSemesterEnrollment StudentSemesterEnrollment { get; set; } = default!;

    public Guid CourseOfferingId { get; set; }
    public CourseOffering CourseOffering { get; set; } = default!;

    public int AttemptNumber { get; set; }
    public int CreditsSnapshot { get; set; }
    public EnrollmentCourseStatus Status { get; set; } = EnrollmentCourseStatus.Enrolled;
    public DateTime EnrolledAt { get; set; }
    public DateTime? DroppedAt { get; set; }
}
