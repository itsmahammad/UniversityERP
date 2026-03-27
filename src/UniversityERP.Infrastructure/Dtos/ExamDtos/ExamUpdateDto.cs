using UniversityERP.Domain.Enums;

namespace UniversityERP.Infrastructure.Dtos.ExamDtos;

public class ExamUpdateDto
{
    public Guid Id { get; set; }
    public Guid CourseOfferingId { get; set; }
    public ExamType ExamType { get; set; }
    public DateOnly ExamDate { get; set; }
    public decimal MaxScore { get; set; }
    public decimal WeightPercentage { get; set; }
    public bool IsActive { get; set; }
}
