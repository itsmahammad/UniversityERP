using UniversityERP.Domain.Entities.Common;
using UniversityERP.Domain.Enums;

namespace UniversityERP.Domain.Entities;

public class AcademicProgram : BaseAuditableEntity
{
    public string Name { get; set; } = default!;
    public ProgramLevel Level { get; set; }
    public int MaxYears { get; set; }
    public bool IsActive { get; set; } = true;

    public Guid DepartmentId { get; set; }
    public Department Department { get; set; } = default!;

    public ICollection<Student> Students { get; set; } = new List<Student>();
    public ICollection<ProgramCourse> ProgramCourses { get; set; } = new List<ProgramCourse>();
    public ICollection<StudentSemesterEnrollment> StudentSemesterEnrollments { get; set; } = new List<StudentSemesterEnrollment>();
}
