using UniversityERP.Domain.Enums;

namespace UniversityERP.Infrastructure.Dtos.ExamDtos;

public class ExamCreateDto
{
    public Guid CourseOfferingId { get; set; }
    public ExamType ExamType { get; set; }
    public DateOnly ExamDate { get; set; }
    public decimal MaxScore { get; set; } = 100;
    public decimal WeightPercentage { get; set; }
    public bool IsActive { get; set; } = true;
}
