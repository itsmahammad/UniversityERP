using UniversityERP.Domain.Entities.Common;
using UniversityERP.Domain.Enums;

namespace UniversityERP.Domain.Entities;

public class Exam : BaseAuditableEntity
{
    public Guid CourseOfferingId { get; set; }
    public CourseOffering CourseOffering { get; set; } = default!;

    public ExamType ExamType { get; set; }
    public DateOnly ExamDate { get; set; }
    public decimal MaxScore { get; set; } = 100;
    public decimal WeightPercentage { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<ExamResult> ExamResults { get; set; } = new List<ExamResult>();
}
