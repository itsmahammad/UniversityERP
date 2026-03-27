namespace UniversityERP.Infrastructure.Dtos.ProgramCourseDtos;

public class ProgramCourseCreateDto
{
    public Guid AcademicProgramId { get; set; }
    public Guid AcademicCourseId { get; set; }
    public int SemesterNumber { get; set; }
    public bool IsCore { get; set; } = true;
}
