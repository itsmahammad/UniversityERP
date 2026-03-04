using UniversityERP.Domain.Enums;

namespace UniversityERP.Infrastructure.Dtos.StudentDtos;

public class StudentCreateDto
{
    public Guid UserId { get; set; }
    public Guid AcademicProgramId { get; set; }
    public int EnrollmentYear { get; set; }
    public StudentStatus Status { get; set; } = StudentStatus.Active;
}