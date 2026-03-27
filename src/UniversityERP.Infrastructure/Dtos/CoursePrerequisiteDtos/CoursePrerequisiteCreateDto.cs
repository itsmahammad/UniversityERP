namespace UniversityERP.Infrastructure.Dtos.CoursePrerequisiteDtos;

public class CoursePrerequisiteCreateDto
{
    public Guid AcademicCourseId { get; set; }
    public Guid PrerequisiteAcademicCourseId { get; set; }
}
