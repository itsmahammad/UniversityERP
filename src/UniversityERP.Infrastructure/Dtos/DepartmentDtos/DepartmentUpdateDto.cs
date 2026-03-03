namespace UniversityERP.Infrastructure.Dtos.DepartmentDtos;

public class DepartmentUpdateDto
{
    public Guid Id { get; set; }
    public Guid FacultyId { get; set; }
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
    public bool IsActive { get; set; }
}