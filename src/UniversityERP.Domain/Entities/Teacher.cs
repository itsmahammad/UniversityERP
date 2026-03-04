using UniversityERP.Domain.Entities.Common;
using UniversityERP.Domain.Enums;

namespace UniversityERP.Domain.Entities;

public class Teacher : BaseAuditableEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public Guid DepartmentId { get; set; }
    public Department Department { get; set; } = default!;

    public TeacherTitle Title { get; set; }
    public DateOnly HireDate { get; set; }

    public bool IsActive { get; set; } = true;
}