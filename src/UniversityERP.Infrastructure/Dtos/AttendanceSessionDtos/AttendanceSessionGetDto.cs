using UniversityERP.Infrastructure.Dtos.AttendanceRecordDtos;

namespace UniversityERP.Infrastructure.Dtos.AttendanceSessionDtos;

public class AttendanceSessionGetDto
{
    public Guid Id { get; set; }
    public Guid CourseOfferingId { get; set; }
    public string AcademicCourseCode { get; set; } = default!;
    public string AcademicCourseName { get; set; } = default!;
    public string TeacherFullName { get; set; } = default!;
    public string Section { get; set; } = string.Empty;
    public DateOnly SessionDate { get; set; }
    public string? Topic { get; set; }
    public bool IsActive { get; set; }
    public List<AttendanceRecordGetDto> AttendanceRecords { get; set; } = [];
}
