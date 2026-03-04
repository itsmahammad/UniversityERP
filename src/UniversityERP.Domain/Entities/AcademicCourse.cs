using UniversityERP.Domain.Entities.Common;

namespace UniversityERP.Domain.Entities;

public class AcademicCourse : BaseAuditableEntity
{
    public Guid OwningDepartmentId { get; set; }
    public Department OwningDepartment { get; set; } = default!;

    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Description { get; set; }

    public int EctsCredits { get; set; }
    public bool IsActive { get; set; } = true;
}