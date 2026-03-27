using UniversityERP.Domain.Entities.Common;

namespace UniversityERP.Domain.Entities;

public class CoursePrerequisite : BaseAuditableEntity
{
    public Guid AcademicCourseId { get; set; }
    public AcademicCourse AcademicCourse { get; set; } = default!;

    public Guid PrerequisiteAcademicCourseId { get; set; }
    public AcademicCourse PrerequisiteAcademicCourse { get; set; } = default!;
}
