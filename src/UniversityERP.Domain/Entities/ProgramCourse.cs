using UniversityERP.Domain.Entities.Common;

namespace UniversityERP.Domain.Entities;

public class ProgramCourse : BaseAuditableEntity
{
    public Guid AcademicProgramId { get; set; }
    public AcademicProgram AcademicProgram { get; set; } = default!;

    public Guid AcademicCourseId { get; set; }
    public AcademicCourse AcademicCourse { get; set; } = default!;

    public int SemesterNumber { get; set; }
    public bool IsCore { get; set; } = true;
}
