using UniversityERP.Domain.Entities.Common;
using UniversityERP.Domain.Enums;

namespace UniversityERP.Domain.Entities;

public class StudentSemesterEnrollment : BaseAuditableEntity
{
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = default!;

    public Guid SemesterId { get; set; }
    public Semester Semester { get; set; } = default!;

    public Guid AcademicProgramId { get; set; }
    public AcademicProgram AcademicProgram { get; set; } = default!;

    public StudentStatus StudentStatus { get; set; }
    public int MaxCredits { get; set; }
    public decimal? StartingCgpa { get; set; }
    public StudentSemesterEnrollmentStatus Status { get; set; } = StudentSemesterEnrollmentStatus.Draft;
    public string? Notes { get; set; }
}
