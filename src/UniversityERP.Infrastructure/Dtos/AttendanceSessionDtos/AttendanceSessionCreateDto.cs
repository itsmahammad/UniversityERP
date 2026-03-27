namespace UniversityERP.Infrastructure.Dtos.AttendanceSessionDtos;

public class AttendanceSessionCreateDto
{
    public Guid CourseOfferingId { get; set; }
    public DateOnly SessionDate { get; set; }
    public string? Topic { get; set; }
    public bool IsActive { get; set; } = true;
}
