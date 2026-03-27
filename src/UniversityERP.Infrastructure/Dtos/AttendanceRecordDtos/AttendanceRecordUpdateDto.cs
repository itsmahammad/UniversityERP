using UniversityERP.Domain.Enums;

namespace UniversityERP.Infrastructure.Dtos.AttendanceRecordDtos;

public class AttendanceRecordUpdateDto
{
    public Guid Id { get; set; }
    public Guid EnrollmentCourseId { get; set; }
    public AttendanceStatus Status { get; set; }
    public string? Note { get; set; }
}
