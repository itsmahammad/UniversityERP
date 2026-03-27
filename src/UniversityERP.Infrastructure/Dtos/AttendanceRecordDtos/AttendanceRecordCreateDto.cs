using UniversityERP.Domain.Enums;

namespace UniversityERP.Infrastructure.Dtos.AttendanceRecordDtos;

public class AttendanceRecordCreateDto
{
    public Guid EnrollmentCourseId { get; set; }
    public AttendanceStatus Status { get; set; }
    public string? Note { get; set; }
}
