using UniversityERP.Domain.Enums;

namespace UniversityERP.Infrastructure.Dtos.AcademicProgramDtos;

public class AcademicProgramUpdateDto
{
    public Guid Id { get; set; }
    public Guid DepartmentId { get; set; }
    public string Name { get; set; } = default!;
    public ProgramLevel Level { get; set; }
    public int MaxYears { get; set; }
    public bool IsActive { get; set; }
}