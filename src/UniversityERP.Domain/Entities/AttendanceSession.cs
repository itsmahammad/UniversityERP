using UniversityERP.Domain.Entities.Common;

namespace UniversityERP.Domain.Entities;

public class AttendanceSession : BaseAuditableEntity
{
    public Guid CourseOfferingId { get; set; }
    public CourseOffering CourseOffering { get; set; } = default!;

    public DateOnly SessionDate { get; set; }
    public string? Topic { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();
}
