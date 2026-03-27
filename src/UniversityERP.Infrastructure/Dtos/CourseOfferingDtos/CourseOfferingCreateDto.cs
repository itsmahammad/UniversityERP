namespace UniversityERP.Infrastructure.Dtos.CourseOfferingDtos;

public class CourseOfferingCreateDto
{
    public Guid AcademicCourseId { get; set; }
    public Guid SemesterId { get; set; }
    public Guid TeacherId { get; set; }
    public string? Section { get; set; }
    public bool IsActive { get; set; } = true;
}
