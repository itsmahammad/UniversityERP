namespace UniversityERP.Infrastructure.Dtos.AcademicYearDtos;

public class AcademicYearCreateDto
{
    public string Name { get; set; } = default!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public bool IsActive { get; set; } = true;
}