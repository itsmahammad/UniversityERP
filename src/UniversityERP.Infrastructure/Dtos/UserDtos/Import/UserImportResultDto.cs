namespace UniversityERP.Infrastructure.Dtos.UserDtos.Import;

public class UserImportResultDto
{
    public int TotalRows { get; set; }
    public int CreatedCount { get; set; }
    public int FailedCount { get; set; }
    public List<UserImportRowResultDto> Rows { get; set; } = [];
}