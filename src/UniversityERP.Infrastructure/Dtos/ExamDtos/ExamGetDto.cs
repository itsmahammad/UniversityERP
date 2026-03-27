using UniversityERP.Domain.Enums;

namespace UniversityERP.Infrastructure.Dtos.ExamDtos;

public class ExamGetDto
{
    public Guid Id { get; set; }
    public Guid CourseOfferingId { get; set; }
    public string AcademicCourseCode { get; set; } = default!;
    public string AcademicCourseName { get; set; } = default!;
    public string SemesterName { get; set; } = default!;
    public string TeacherFullName { get; set; } = default!;
    public string Section { get; set; } = string.Empty;
    public ExamType ExamType { get; set; }
    public DateOnly ExamDate { get; set; }
    public decimal MaxScore { get; set; }
    public decimal WeightPercentage { get; set; }
    public bool IsActive { get; set; }
}
