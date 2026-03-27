namespace UniversityERP.Infrastructure.Dtos.GpaDtos;

public class SemesterGpaDto
{
    public Guid StudentId { get; set; }
    public Guid StudentSemesterEnrollmentId { get; set; }
    public Guid SemesterId { get; set; }
    public string SemesterName { get; set; } = default!;
    public int TotalCredits { get; set; }
    public decimal TotalGradePointsWeighted { get; set; }
    public decimal Gpa { get; set; }
    public List<GpaCourseDto> Courses { get; set; } = [];
}
