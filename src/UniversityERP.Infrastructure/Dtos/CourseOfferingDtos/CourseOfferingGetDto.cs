namespace UniversityERP.Infrastructure.Dtos.CourseOfferingDtos;

public class CourseOfferingGetDto
{
    public Guid Id { get; set; }
    public Guid AcademicCourseId { get; set; }
    public string AcademicCourseCode { get; set; } = default!;
    public string AcademicCourseName { get; set; } = default!;
    public int EctsCredits { get; set; }
    public Guid SemesterId { get; set; }
    public string SemesterName { get; set; } = default!;
    public Guid TeacherId { get; set; }
    public string TeacherFullName { get; set; } = default!;
    public string Section { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
