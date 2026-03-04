using UniversityERP.Domain.Enums;

namespace UniversityERP.Infrastructure.Dtos.TeacherDtos;

public class TeacherGetDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid DepartmentId { get; set; }
    public TeacherTitle Title { get; set; }
    public DateOnly HireDate { get; set; }
    public bool IsActive { get; set; }
}