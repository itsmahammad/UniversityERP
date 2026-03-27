namespace UniversityERP.Infrastructure.Dtos.CourseOfferingDtos;

public class CourseOfferingUpdateDto
{
    public Guid Id { get; set; }
    public Guid AcademicCourseId { get; set; }
    public Guid SemesterId { get; set; }
    public Guid TeacherId { get; set; }
    public string? Section { get; set; }
    public bool IsActive { get; set; }
}
