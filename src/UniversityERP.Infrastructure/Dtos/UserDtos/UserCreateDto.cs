using UniversityERP.Domain.Enums;

namespace UniversityERP.Infrastructure.Dtos.UserDtos;

public class UserCreateDto
{
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public UserRole Role { get; set; }
    public bool IsActive { get; set; } = true;
    public string? PositionTitle { get; set; } // optional display
}