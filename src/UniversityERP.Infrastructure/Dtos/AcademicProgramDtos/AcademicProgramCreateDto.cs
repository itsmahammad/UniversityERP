using UniversityERP.Domain.Enums;

namespace UniversityERP.Infrastructure.Dtos.AcademicProgramDtos;

public class AcademicProgramCreateDto
{
    public Guid DepartmentId { get; set; }
    public string Name { get; set; } = default!;
    public ProgramLevel Level { get; set; }
    public int MaxYears { get; set; }
    public bool IsActive { get; set; } = true;
}