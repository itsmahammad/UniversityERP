namespace UniversityERP.Infrastructure.Dtos.DepartmentDtos;

public class DepartmentCreateDto
{
    public Guid FacultyId { get; set; }
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
    public bool IsActive { get; set; } = true;
}