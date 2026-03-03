namespace UniversityERP.Infrastructure.Dtos.FacultyDtos;

public class FacultyUpdateDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
    public bool IsActive { get; set; }
}