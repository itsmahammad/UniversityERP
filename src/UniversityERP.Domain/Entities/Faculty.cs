using UniversityERP.Domain.Entities.Common;

namespace UniversityERP.Domain.Entities;

public class Faculty : BaseAuditableEntity
{
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
    public bool IsActive { get; set; } = true;

    public ICollection<Department> Departments { get; set; } = new List<Department>();
}