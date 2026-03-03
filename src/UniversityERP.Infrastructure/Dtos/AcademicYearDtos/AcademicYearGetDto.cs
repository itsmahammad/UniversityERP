namespace UniversityERP.Infrastructure.Dtos.AcademicYearDtos;

public class AcademicYearGetDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public bool IsActive { get; set; }
}