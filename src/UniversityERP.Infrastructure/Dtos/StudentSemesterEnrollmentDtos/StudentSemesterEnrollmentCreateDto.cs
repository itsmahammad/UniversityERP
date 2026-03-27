namespace UniversityERP.Infrastructure.Dtos.StudentSemesterEnrollmentDtos;

public class StudentSemesterEnrollmentCreateDto
{
    public Guid StudentId { get; set; }
    public Guid SemesterId { get; set; }
    public decimal? StartingCgpa { get; set; }
    public string? Notes { get; set; }
}
