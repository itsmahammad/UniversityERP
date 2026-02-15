using UniversityERP.Domain.Entities.Common;
using UniversityERP.Domain.Enums;

namespace UniversityERP.Domain.Entities;

public class User : BaseAuditableEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string FinCode { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? PersonalEmail { get; set; }
    public string PasswordHash { get; set; } = default!;

    public UserRole Role { get; set; } = UserRole.Student;
    public string? PositionTitle { get; set; }

    public bool IsActive { get; set; } = true;
}