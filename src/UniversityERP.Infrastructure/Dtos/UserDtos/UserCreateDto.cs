using UniversityERP.Domain.Enums;

namespace UniversityERP.Infrastructure.Dtos.UserDtos;

public class UserCreateDto
{
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;          // university login email
    public string? PersonalEmail { get; set; }             // where to send credentials

    public UserRole Role { get; set; }
    public bool IsActive { get; set; } = true;
    public string? PositionTitle { get; set; }
}