namespace UniversityERP.Infrastructure.Dtos.CoursePrerequisiteDtos;

public class CoursePrerequisiteUpdateDto
{
    public Guid Id { get; set; }
    public Guid AcademicCourseId { get; set; }
    public Guid PrerequisiteAcademicCourseId { get; set; }
}
