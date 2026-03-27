using UniversityERP.Domain.Entities.Common;

namespace UniversityERP.Domain.Entities;

public class ExamResult : BaseAuditableEntity
{
    public Guid EnrollmentCourseId { get; set; }
    public EnrollmentCourse EnrollmentCourse { get; set; } = default!;

    public Guid ExamId { get; set; }
    public Exam Exam { get; set; } = default!;

    public decimal NumericScore { get; set; }
}
