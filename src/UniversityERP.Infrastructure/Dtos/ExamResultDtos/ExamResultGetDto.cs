using UniversityERP.Domain.Enums;

namespace UniversityERP.Infrastructure.Dtos.ExamResultDtos;

public class ExamResultGetDto
{
    public Guid Id { get; set; }
    public Guid EnrollmentCourseId { get; set; }
    public Guid ExamId { get; set; }
    public ExamType ExamType { get; set; }
    public DateOnly ExamDate { get; set; }
    public decimal NumericScore { get; set; }
    public decimal MaxScore { get; set; }
    public decimal WeightPercentage { get; set; }
    public decimal WeightedContribution { get; set; }
}
