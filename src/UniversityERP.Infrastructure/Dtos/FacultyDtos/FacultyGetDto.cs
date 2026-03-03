namespace UniversityERP.Infrastructure.Dtos.FacultyDtos;

public class FacultyGetDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
    public int DepartmentCount { get; set; }
}