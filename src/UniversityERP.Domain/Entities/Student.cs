using UniversityERP.Domain.Entities.Common;
using UniversityERP.Domain.Enums;

namespace UniversityERP.Domain.Entities;

public class Student : BaseAuditableEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public Guid AcademicProgramId { get; set; }
    public AcademicProgram AcademicProgram { get; set; } = default!;

    public int EnrollmentYear { get; set; }
    public StudentStatus Status { get; set; } = StudentStatus.Active;
}