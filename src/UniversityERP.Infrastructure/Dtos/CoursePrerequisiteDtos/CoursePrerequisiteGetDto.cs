namespace UniversityERP.Infrastructure.Dtos.CoursePrerequisiteDtos;

public class CoursePrerequisiteGetDto
{
    public Guid Id { get; set; }
    public Guid AcademicCourseId { get; set; }
    public string AcademicCourseCode { get; set; } = default!;
    public string AcademicCourseName { get; set; } = default!;
    public Guid PrerequisiteAcademicCourseId { get; set; }
    public string PrerequisiteAcademicCourseCode { get; set; } = default!;
    public string PrerequisiteAcademicCourseName { get; set; } = default!;
}
