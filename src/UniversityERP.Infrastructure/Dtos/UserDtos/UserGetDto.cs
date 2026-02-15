using UniversityERP.Domain.Enums;

namespace UniversityERP.Infrastructure.Dtos.UserDtos;

public class UserGetDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? PersonalEmail { get; set; }
    public UserRole Role { get; set; }
    public bool IsActive { get; set; }
    public string? PositionTitle { get; set; }
}