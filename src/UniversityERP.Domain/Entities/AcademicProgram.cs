using UniversityERP.Domain.Entities.Common;
using UniversityERP.Domain.Enums;

namespace UniversityERP.Domain.Entities;

public class AcademicProgram : BaseAuditableEntity
{
    public string Name { get; set; } = default!;
    public ProgramLevel Level { get; set; }
    public int MaxYears { get; set; }
    public bool IsActive { get; set; } = true;

    public Guid DepartmentId { get; set; }
    public Department Department { get; set; } = default!;
}