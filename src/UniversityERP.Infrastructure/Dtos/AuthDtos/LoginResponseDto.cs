namespace UniversityERP.Infrastructure.Dtos.AuthDtos;

public class LoginResponseDto
{
    public string AccessToken { get; set; } = default!;
    public DateTime ExpiresAtUtc { get; set; }

    public Guid UserId { get; set; }
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Role { get; set; } = default!;
}