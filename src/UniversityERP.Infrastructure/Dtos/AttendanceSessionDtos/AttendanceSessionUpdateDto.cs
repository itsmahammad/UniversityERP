namespace UniversityERP.Infrastructure.Dtos.AttendanceSessionDtos;

public class AttendanceSessionUpdateDto
{
    public Guid Id { get; set; }
    public Guid CourseOfferingId { get; set; }
    public DateOnly SessionDate { get; set; }
    public string? Topic { get; set; }
    public bool IsActive { get; set; }
}
