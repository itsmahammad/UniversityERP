using UniversityERP.Domain.Enums;

namespace UniversityERP.Infrastructure.Dtos.TeacherDtos;

public class TeacherUpdateDto
{
    public Guid Id { get; set; }
    public Guid DepartmentId { get; set; }
    public TeacherTitle Title { get; set; }
    public DateOnly HireDate { get; set; }
    public bool IsActive { get; set; }
}