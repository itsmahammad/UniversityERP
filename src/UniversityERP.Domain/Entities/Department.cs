using UniversityERP.Domain.Entities.Common;

namespace UniversityERP.Domain.Entities;

public class Department : BaseAuditableEntity
{
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
    public bool IsActive { get; set; } = true;

    public Guid FacultyId { get; set; }
    public Faculty Faculty { get; set; } = default!;

    public ICollection<AcademicProgram> AcademicPrograms { get; set; } = new List<AcademicProgram>();
}