namespace UniversityERP.Infrastructure.Dtos.UserDtos;

public class UserUpdateDto
{
    public string FullName { get; set; } = default!;
    public string? PersonalEmail { get; set; }
    public string? PositionTitle { get; set; }
}