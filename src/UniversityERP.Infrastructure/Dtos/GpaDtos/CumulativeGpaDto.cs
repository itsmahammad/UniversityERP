namespace UniversityERP.Infrastructure.Dtos.GpaDtos;

public class CumulativeGpaDto
{
    public Guid StudentId { get; set; }
    public int TotalCredits { get; set; }
    public decimal TotalGradePointsWeighted { get; set; }
    public decimal Gpa { get; set; }
    public int CompletedCoursesCount { get; set; }
    public List<SemesterGpaDto> Semesters { get; set; } = [];
}
