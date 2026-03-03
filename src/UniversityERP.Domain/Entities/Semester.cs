using UniversityERP.Domain.Entities.Common;
using UniversityERP.Domain.Enums;

namespace UniversityERP.Domain.Entities;

public class Semester : BaseAuditableEntity
{
    public Guid AcademicYearId { get; set; }
    public AcademicYear AcademicYear { get; set; } = default!;

    public SemesterTerm Term { get; set; }

    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }

    public DateOnly RegistrationStart { get; set; }
    public DateOnly RegistrationEnd { get; set; }

    public DateOnly ExamStart { get; set; }
    public DateOnly ExamEnd { get; set; }

    public int MaxCredits { get; set; } = 30;
    public bool IsActive { get; set; } = true;
}