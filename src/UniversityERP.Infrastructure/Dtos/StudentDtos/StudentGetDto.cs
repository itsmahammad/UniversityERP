using UniversityERP.Domain.Enums;

namespace UniversityERP.Infrastructure.Dtos.StudentDtos;

public class StudentGetDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid AcademicProgramId { get; set; }
    public int EnrollmentYear { get; set; }
    public StudentStatus Status { get; set; }
}