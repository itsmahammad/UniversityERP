namespace UniversityERP.Infrastructure.Dtos.GpaDtos;

public class GpaCourseDto
{
    public Guid EnrollmentCourseId { get; set; }
    public Guid AcademicCourseId { get; set; }
    public string AcademicCourseCode { get; set; } = default!;
    public string AcademicCourseName { get; set; } = default!;
    public int Credits { get; set; }
    public decimal FinalNumericScore { get; set; }
    public string LetterGrade { get; set; } = default!;
    public decimal GradePoint { get; set; }
}
