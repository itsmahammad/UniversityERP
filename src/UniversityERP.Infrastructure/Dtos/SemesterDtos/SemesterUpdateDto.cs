using UniversityERP.Domain.Enums;

namespace UniversityERP.Infrastructure.Dtos.SemesterDtos;

public class SemesterUpdateDto
{
    public Guid Id { get; set; }
    public Guid AcademicYearId { get; set; }
    public SemesterTerm Term { get; set; }

    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }

    public DateOnly RegistrationStart { get; set; }
    public DateOnly RegistrationEnd { get; set; }

    public DateOnly ExamStart { get; set; }
    public DateOnly ExamEnd { get; set; }

    public int MaxCredits { get; set; }
    public bool IsActive { get; set; }
}