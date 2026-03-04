using UniversityERP.Domain.Enums;

namespace UniversityERP.Infrastructure.Dtos.TeacherDtos;

public class TeacherCreateDto
{
    public Guid UserId { get; set; }
    public Guid DepartmentId { get; set; }
    public TeacherTitle Title { get; set; }
    public DateOnly HireDate { get; set; }
    public bool IsActive { get; set; } = true;
}