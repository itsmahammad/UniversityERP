using UniversityERP.Domain.Entities.Common;

namespace UniversityERP.Domain.Entities;

public class Faculty : BaseAuditableEntity
{
    public string Name { get; set; } = string.Empty;
    // Navigation properties can be added here if needed, e.g.:
    // public ICollection<Department> Departments { get; set; } = new List<Department>();
}
