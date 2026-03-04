namespace UniversityERP.Infrastructure.Dtos.AcademicCourseDtos;

public class AcademicCourseGetDto
{
    public Guid Id { get; set; }
    public Guid OwningDepartmentId { get; set; }
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public int EctsCredits { get; set; }
    public bool IsActive { get; set; }
}