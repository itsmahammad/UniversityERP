namespace UniversityERP.Infrastructure.Options;

public class EmailOptions
{
    public string Host { get; set; } = default!;
    public int Port { get; set; } = 587;
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string FromEmail { get; set; } = default!;
    public string FromName { get; set; } = "University ERP";
    public bool UseStartTls { get; set; } = true;
}