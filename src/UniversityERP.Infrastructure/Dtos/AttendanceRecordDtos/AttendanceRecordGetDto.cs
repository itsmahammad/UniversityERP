using UniversityERP.Domain.Enums;

namespace UniversityERP.Infrastructure.Dtos.AttendanceRecordDtos;

public class AttendanceRecordGetDto
{
    public Guid Id { get; set; }
    public Guid EnrollmentCourseId { get; set; }
    public Guid StudentId { get; set; }
    public string StudentFullName { get; set; } = default!;
    public AttendanceStatus Status { get; set; }
    public string? Note { get; set; }
}
