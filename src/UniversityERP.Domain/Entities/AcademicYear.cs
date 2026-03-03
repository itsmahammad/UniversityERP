using UniversityERP.Domain.Entities.Common;

namespace UniversityERP.Domain.Entities;

public class AcademicYear : BaseAuditableEntity
{
    public string Name { get; set; } = default!; // 2025/2026
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<Semester> Semesters { get; set; } = new List<Semester>();
}