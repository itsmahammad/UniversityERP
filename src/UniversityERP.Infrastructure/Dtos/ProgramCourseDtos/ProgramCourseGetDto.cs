namespace UniversityERP.Infrastructure.Dtos.ProgramCourseDtos;

public class ProgramCourseGetDto
{
    public Guid Id { get; set; }
    public Guid AcademicProgramId { get; set; }
    public string AcademicProgramName { get; set; } = default!;
    public Guid AcademicCourseId { get; set; }
    public string AcademicCourseCode { get; set; } = default!;
    public string AcademicCourseName { get; set; } = default!;
    public int EctsCredits { get; set; }
    public int SemesterNumber { get; set; }
    public bool IsCore { get; set; }
}
