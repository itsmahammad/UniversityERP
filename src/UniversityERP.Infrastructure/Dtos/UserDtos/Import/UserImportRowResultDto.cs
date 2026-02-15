namespace UniversityERP.Infrastructure.Dtos.UserDtos.Import;

public class UserImportRowResultDto
{
    public int RowNumber { get; set; }
    public bool Success { get; set; }

    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Role { get; set; } = "";

    public bool? IsActive { get; set; }
    public string? PositionTitle { get; set; }

    public string? TempPassword { get; set; } // only if Success=true
    public string? Error { get; set; }        // only if Success=false
}