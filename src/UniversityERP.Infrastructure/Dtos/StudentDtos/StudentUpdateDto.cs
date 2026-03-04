using UniversityERP.Domain.Enums;

namespace UniversityERP.Infrastructure.Dtos.StudentDtos;

public class StudentUpdateDto
{
    public Guid Id { get; set; }
    public Guid AcademicProgramId { get; set; }
    public int EnrollmentYear { get; set; }
    public StudentStatus Status { get; set; }
}