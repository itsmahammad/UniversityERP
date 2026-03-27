using UniversityERP.Domain.Entities.Common;
using UniversityERP.Domain.Enums;

namespace UniversityERP.Domain.Entities;

public class AttendanceRecord : BaseAuditableEntity
{
    public Guid AttendanceSessionId { get; set; }
    public AttendanceSession AttendanceSession { get; set; } = default!;

    public Guid EnrollmentCourseId { get; set; }
    public EnrollmentCourse EnrollmentCourse { get; set; } = default!;

    public AttendanceStatus Status { get; set; }
    public string? Note { get; set; }
}
