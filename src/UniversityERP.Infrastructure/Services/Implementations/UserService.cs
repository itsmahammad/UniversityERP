using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Domain.Entities;
using UniversityERP.Domain.Enums;
using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.Common;
using UniversityERP.Infrastructure.Dtos.UserDtos;
using UniversityERP.Infrastructure.Dtos.UserDtos.Import;
using UniversityERP.Infrastructure.EmailTemplates;
using UniversityERP.Infrastructure.Options;
using UniversityERP.Infrastructure.Services.Abstractions;

namespace UniversityERP.Infrastructure.Services.Implementations;

internal class UserService : IUserService
{
    private readonly IUserRepository _users;
    private readonly IHttpContextAccessor _http;
    private readonly PasswordHasher<User> _hasher = new();
    private readonly ILogger<UserService> _logger;
    private readonly IEmailSender _email;
    private readonly string _uniEmailDomain;

    public UserService(IUserRepository users, IHttpContextAccessor http, IEmailSender email, ILogger<UserService> logger, IOptions<UniversityEmailOptions> uniEmailOptions)
    {
        _users = users;
        _http = http;
        _email = email;
        _logger = logger;
        _uniEmailDomain = uniEmailOptions.Value.Domain;
    }

    private Guid? CurrentUserId()
    {
        var sub = _http.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? _http.HttpContext?.User?.FindFirstValue("sub");

        return Guid.TryParse(sub, out var id) ? id : null;
    }

    public async Task<ResultDto<UserGetDto>> CreateAsync(UserCreateDto dto)
    {
        // Normalize FinCode
        var fin = dto.FinCode.Trim().ToUpperInvariant();

        // Validate FinCode format and length
        if (string.IsNullOrWhiteSpace(fin) || fin.Length < 5 || fin.Length > 16)
        {
            return new ResultDto<UserGetDto>
            {
                StatusCode = 400,
                IsSucced = false,
                Message = "FinCode must be between 5 and 16 characters."
            };
        }

        // Check FinCode uniqueness (including soft-deleted)
        if (await _users.ExistsByFinCodeAsync(fin, ignoreQueryFilter: true))
        {
            return new ResultDto<UserGetDto>
            {
                StatusCode = 409,
                IsSucced = false,
                Message = "FinCode already exists."
            };
        }

        // Generate university email from FinCode
        var email = $"{fin}@{_uniEmailDomain}".ToLowerInvariant();

        // Check email uniqueness (including soft-deleted)
        if (await _users.ExistsByEmailAsync(email, ignoreQueryFilter: true))
        {
            return new ResultDto<UserGetDto>
            {
                StatusCode = 409,
                IsSucced = false,
                Message = "University email already exists."
            };
        }

        var personalEmail = string.IsNullOrWhiteSpace(dto.PersonalEmail)
            ? null
            : dto.PersonalEmail.Trim().ToLowerInvariant();

        if (!string.IsNullOrWhiteSpace(personalEmail) && !MailAddress.TryCreate(personalEmail, out _))
        {
            return new ResultDto<UserGetDto>
            {
                StatusCode = 400,
                IsSucced = false,
                Message = "PersonalEmail is not a valid email address."
            };
        }

        var tempPassword = GenerateTempPassword();

        var user = new User
        {
            FinCode = fin,
            FullName = dto.FullName.Trim(),
            Email = email,
            PersonalEmail = personalEmail,
            Role = dto.Role,
            IsActive = dto.IsActive,
            PositionTitle = string.IsNullOrWhiteSpace(dto.PositionTitle) ? null : dto.PositionTitle.Trim()
        };

        user.PasswordHash = _hasher.HashPassword(user, tempPassword);

        await _users.AddAsync(user);
        await _users.SaveChangesAsync();

        // Send credentials to personal email if provided
        if (!string.IsNullOrWhiteSpace(user.PersonalEmail))
        {
            try
            {
                var subject = "Your University Account Credentials";
                var body = UserEmails.Credentials(user.FullName, user.Email, tempPassword);
                await _email.SendAsync(user.PersonalEmail, subject, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SMTP send failed to {PersonalEmail}", user.PersonalEmail);
            }
        }

        return new ResultDto<UserGetDto>
        {
            StatusCode = 201,
            IsSucced = true,
            Message = string.IsNullOrWhiteSpace(user.PersonalEmail)
                ? "User created successfully. PersonalEmail not provided, credentials were not emailed."
                : "User created successfully. Credentials sent to personal email.",
            Data = new UserGetDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PersonalEmail = user.PersonalEmail,
                Role = user.Role,
                IsActive = user.IsActive,
                PositionTitle = user.PositionTitle
            }
        };
    }

    public async Task<ResultDto<PagedResponseDto<UserGetDto>>> GetPagedAsync(int page, int pageSize, string? search,
        UserRole? role, bool? isActive)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : pageSize;
        pageSize = pageSize > 100 ? 100 : pageSize;

        var query = _users.GetAll().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();
            query = query.Where(x =>
                x.FullName.ToLower().Contains(s) ||
                x.Email.ToLower().Contains(s));
        }

        if (role.HasValue)
            query = query.Where(x => x.Role == role.Value);

        if (isActive.HasValue)
            query = query.Where(x => x.IsActive == isActive.Value);

        var total = await query.CountAsync();

        var items = await query
            .OrderBy(x => x.FullName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new UserGetDto
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                Role = x.Role,
                PersonalEmail = x.PersonalEmail,
                IsActive = x.IsActive,
                PositionTitle = x.PositionTitle
            })
            .ToListAsync();

        return new ResultDto<PagedResponseDto<UserGetDto>>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = new PagedResponseDto<UserGetDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = total,
                Items = items
            }
        };
    }

    public async Task<ResultDto<UserGetDto>> GetByIdAsync(Guid id)
    {
        var user = await _users.GetAsync(x => x.Id == id);

        if (user is null)
        {
            return new ResultDto<UserGetDto>
            {
                StatusCode = 404,
                IsSucced = false,
                Message = "User not found."
            };
        }

        return new ResultDto<UserGetDto>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = new UserGetDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PersonalEmail = user.PersonalEmail,
                Role = user.Role,
                IsActive = user.IsActive,
                PositionTitle = user.PositionTitle
            }
        };
    }
    public async Task<ResultDto> ActivateAsync(Guid id)
    {
        var user = await _users.GetAsync(x => x.Id == id);
        if (user is null)
            return new ResultDto(404, false, "User not found.");

        if (user.IsActive)
            return new ResultDto(200, true, "User already active.");

        user.IsActive = true;
        _users.Update(user);
        await _users.SaveChangesAsync();

        return new ResultDto(200, true, "User activated successfully.");
    }

    public async Task<ResultDto> DeactivateAsync(Guid id)
    {
        var currentId = CurrentUserId();
        if (currentId.HasValue && currentId.Value == id)
            return new ResultDto(400, false, "You cannot deactivate your own account.");

        var user = await _users.GetAsync(x => x.Id == id);
        if (user is null)
            return new ResultDto(404, false, "User not found.");

        if (!user.IsActive)
            return new ResultDto(200, true, "User already inactive.");

        user.IsActive = false;
        _users.Update(user);
        await _users.SaveChangesAsync();

        return new ResultDto(200, true, "User deactivated successfully.");
    }

    public async Task<ResultDto> ChangeRoleAsync(Guid id, ChangeRoleDto dto)
    {
        var user = await _users.GetAsync(x => x.Id == id);
        if (user is null)
            return new ResultDto(404, false, "User not found.");

        // who is calling?
        var caller = _http.HttpContext?.User;
        var callerRole = caller?.FindFirstValue(ClaimTypes.Role);

        var isCallerSuper = string.Equals(callerRole, "SuperAdmin", StringComparison.OrdinalIgnoreCase);

        if (dto.Role == UniversityERP.Domain.Enums.UserRole.SuperAdmin && !isCallerSuper)
            return new ResultDto(403, false, "Only SuperAdmin can assign SuperAdmin role.");

        //prevent superadmin from changing own role
        var currentId = CurrentUserId();
        if (currentId.HasValue && currentId.Value == id && user.Role == UniversityERP.Domain.Enums.UserRole.SuperAdmin && dto.Role != user.Role)
            return new ResultDto(400, false, "SuperAdmin cannot change their own role.");

        if (user.Role == dto.Role)
            return new ResultDto(200, true, "Role is already set.");

        user.Role = dto.Role;
        _users.Update(user);
        await _users.SaveChangesAsync();

        return new ResultDto(200, true, "Role updated successfully.");
    }

    public async Task<ResultDto> ResetPasswordAsync(Guid id, ResetPasswordDto dto)
    {
        var user = await _users.GetAsync(x => x.Id == id);
        if (user is null)
            return new ResultDto(404, false, "User not found.");

        user.PasswordHash = _hasher.HashPassword(user, dto.NewPassword);

        _users.Update(user);
        await _users.SaveChangesAsync();

        if (!string.IsNullOrWhiteSpace(user.PersonalEmail))
        {
            try
            {
                var subject = "Your password has been reset";
                var body = UserEmails.PasswordReset(user.FullName, dto.NewPassword);
                await _email.SendAsync(user.PersonalEmail, subject, body);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "SMTP send failed to {PersonalEmail}", user.PersonalEmail);
            }
        }

        return new ResultDto(200, true, "Password reset successfully.");
    }

    public async Task<ResultDto<UserGetDto>> GetMeAsync()
    {
        var userId = CurrentUserId();
        if (!userId.HasValue)
            return new ResultDto<UserGetDto>
            {
                StatusCode = 401,
                IsSucced = false,
                Message = "Unauthorized."
            };


        var user = await _users.GetAsync(x => x.Id == userId.Value);
        if (user is null)
            return new ResultDto<UserGetDto>
            {
                StatusCode = 404,
                IsSucced = false,
                Message = "User not found."
            };


        return new ResultDto<UserGetDto>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = new UserGetDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PersonalEmail = user.PersonalEmail,
                Role = user.Role,
                IsActive = user.IsActive,
                PositionTitle = user.PositionTitle
            }
        };
    }
    public async Task<ResultDto> ChangeMyPasswordAsync(ChangePasswordDto dto)
    {
        var userId = CurrentUserId();
        if (!userId.HasValue)
            return new ResultDto(401, false, "Unauthorized.");

        var user = await _users.GetAsync(x => x.Id == userId.Value);
        if (user is null)
            return new ResultDto(404, false, "User not found.");

        if (!user.IsActive)
            return new ResultDto(403, false, "Account is inactive.");

        var verify = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.CurrentPassword);
        if (verify == PasswordVerificationResult.Failed)
            return new ResultDto(400, false, "Current password is incorrect.");

        user.PasswordHash = _hasher.HashPassword(user, dto.NewPassword);

        _users.Update(user);
        await _users.SaveChangesAsync();

        return new ResultDto(200, true, "Password changed successfully.");
    }

    private static string GenerateTempPassword(int length = 14)
    {
        const string upper = "ABCDEFGHJKLMNPQRSTUVWXYZ";
        const string lower = "abcdefghijkmnopqrstuvwxyz";
        const string digits = "23456789";
        const string symbols = "!@#$%&*?";

        var all = upper + lower + digits + symbols;

        Span<char> pwd = stackalloc char[length];

        // guarantee complexity
        pwd[0] = upper[RandomNumberGenerator.GetInt32(upper.Length)];
        pwd[1] = lower[RandomNumberGenerator.GetInt32(lower.Length)];
        pwd[2] = digits[RandomNumberGenerator.GetInt32(digits.Length)];
        pwd[3] = symbols[RandomNumberGenerator.GetInt32(symbols.Length)];

        for (int i = 4; i < length; i++)
            pwd[i] = all[RandomNumberGenerator.GetInt32(all.Length)];

        // shuffle
        for (int i = pwd.Length - 1; i > 0; i--)
        {
            int j = RandomNumberGenerator.GetInt32(i + 1);
            (pwd[i], pwd[j]) = (pwd[j], pwd[i]);
        }

        return new string(pwd);
    }

    public async Task<ResultDto<UserImportResultDto>> ImportUsersFromExcelAsync(IFormFile file)
{
    if (file is null || file.Length == 0)
        return new ResultDto<UserImportResultDto>
        {
            StatusCode = 400,
            IsSucced = false,
            Message = "File is required."
        };

    if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
        return new ResultDto<UserImportResultDto>
        {
            StatusCode = 400,
            IsSucced = false,
            Message = "Only .xlsx files are supported."
        };

    using var ms = new MemoryStream();
    await file.CopyToAsync(ms);
    ms.Position = 0;

    using var wb = new XLWorkbook(ms);
    var ws = wb.Worksheets.FirstOrDefault();
    if (ws is null)
        return new ResultDto<UserImportResultDto>
        {
            StatusCode = 400,
            IsSucced = false,
            Message = "Excel file has no worksheets."
        };

    var lastRow = ws.LastRowUsed()?.RowNumber() ?? 1;
    if (lastRow < 2)
        return new ResultDto<UserImportResultDto>
        {
            StatusCode = 400,
            IsSucced = false,
            Message = "Excel file has no data rows."
        };

    // Existing FinCodes and Emails (ignore query filter so even deleted users block duplicates)
    var existingFinCodes = await _users.GetAll(ignoreQueryFilter: true)
        .AsNoTracking()
        .Select(x => x.FinCode)
        .ToListAsync();

    var existingEmails = await _users.GetAll(ignoreQueryFilter: true)
        .AsNoTracking()
        .Select(x => x.Email)
        .ToListAsync();

    var existingFinSet = new HashSet<string>(existingFinCodes, StringComparer.OrdinalIgnoreCase);
    var existingEmailSet = new HashSet<string>(existingEmails, StringComparer.OrdinalIgnoreCase);
    var seenFinInFile = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

    var result = new UserImportResultDto { TotalRows = lastRow - 1 };

    // we keep tempPassword in memory only
    var toInsert = new List<(User user, string tempPassword, UserImportRowResultDto rr)>();

    // Expected headers (row 1):
    // FinCode | FullName | PersonalEmail | Role | IsActive | PositionTitle
    for (int row = 2; row <= lastRow; row++)
    {
        var finCodeRaw = ws.Cell(row, 1).GetString()?.Trim() ?? "";
        var fullName = ws.Cell(row, 2).GetString()?.Trim() ?? "";
        var personalEmailRaw = ws.Cell(row, 3).GetString()?.Trim();
        var roleRaw = ws.Cell(row, 4).GetString()?.Trim() ?? "";
        var isActiveRaw = ws.Cell(row, 5).GetString()?.Trim() ?? "";
        var positionTitle = ws.Cell(row, 6).GetString()?.Trim();

        var rr = new UserImportRowResultDto
        {
            RowNumber = row,
            FullName = fullName,
            Email = "", // Will be generated
            Role = roleRaw,
            PositionTitle = string.IsNullOrWhiteSpace(positionTitle) ? null : positionTitle.Trim(),
            PersonalEmail = string.IsNullOrWhiteSpace(personalEmailRaw) ? null : personalEmailRaw.Trim()
        };

        if (string.IsNullOrWhiteSpace(finCodeRaw))
        {
            rr.Success = false;
            rr.Error = "FinCode is required.";
            result.Rows.Add(rr);
            continue;
        }

        var fin = finCodeRaw.Trim().ToUpperInvariant();

        if (string.IsNullOrWhiteSpace(fin))
        {
            rr.Success = false;
            rr.Error = "FinCode is required.";
            result.Rows.Add(rr);
            continue;
        }

        if (!IsValidFinCode(fin))
        {
            rr.Success = false;
            rr.Error = "FinCode must be 5-16 alphanumeric characters.";
            result.Rows.Add(rr);
            continue;
        }

        // Validate FinCode length
        if (fin.Length < 5 || fin.Length > 16)
        {
            rr.Success = false;
            rr.Error = "FinCode must be between 5 and 16 characters.";
            result.Rows.Add(rr);
            continue;
        }

        // Check FinCode duplicates in file
        if (!seenFinInFile.Add(fin))
        {
            rr.Success = false;
            rr.Error = "Duplicate FinCode in file.";
            result.Rows.Add(rr);
            continue;
        }

        // Check FinCode duplicates in DB
        if (existingFinSet.Contains(fin))
        {
            rr.Success = false;
            rr.Error = "FinCode already exists in database.";
            result.Rows.Add(rr);
            continue;
        }

        if (string.IsNullOrWhiteSpace(fullName))
        {
            rr.Success = false;
            rr.Error = "FullName is required.";
            result.Rows.Add(rr);
            continue;
        }

        // Generate university email from FinCode
        var email = $"{fin}@{_uniEmailDomain}".ToLowerInvariant();
        rr.Email = email;

        // Check email uniqueness
        if (existingEmailSet.Contains(email))
        {
            rr.Success = false;
            rr.Error = "Generated email already exists in database.";
            result.Rows.Add(rr);
            continue;
        }

        if (!Enum.TryParse<UserRole>(roleRaw, ignoreCase: true, out var role))
        {
            rr.Success = false;
            rr.Error = $"Invalid role: '{roleRaw}'.";
            result.Rows.Add(rr);
            continue;
        }

        // Parse IsActive (optional, default true)
        var isActive = true;
        if (!string.IsNullOrWhiteSpace(isActiveRaw))
        {
            var v = isActiveRaw.Trim();
            isActive =
                v.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                v.Equals("1") ||
                v.Equals("yes", StringComparison.OrdinalIgnoreCase);
        }
        rr.IsActive = isActive;

        // Validate personal email (optional)
        string? personalEmail = null;

        if (!string.IsNullOrWhiteSpace(personalEmailRaw))
        {
            personalEmail = personalEmailRaw.Trim().ToLowerInvariant();

            if (!MailAddress.TryCreate(personalEmail, out _))
            {
                rr.Success = false;
                rr.Error = "PersonalEmail is not a valid email address.";
                result.Rows.Add(rr);
                continue;
            }
        }

        var tempPassword = GenerateTempPassword();

        var user = new User
        {
            FinCode = fin,
            FullName = fullName.Trim(),
            Email = email,
            PersonalEmail = personalEmail,
            Role = role,
            IsActive = isActive,
            PositionTitle = rr.PositionTitle
        };

        user.PasswordHash = _hasher.HashPassword(user, tempPassword);

        toInsert.Add((user, tempPassword, rr));
    }

    // Save valid users
    foreach (var item in toInsert)
        await _users.AddAsync(item.user);

    if (toInsert.Count > 0)
        await _users.SaveChangesAsync();

    // Build result rows (email if possible)
    foreach (var item in toInsert)
    {
        item.rr.Success = true;

        if (!string.IsNullOrWhiteSpace(item.user.PersonalEmail))
        {
            try
            {
                var subject = "Your University Account Credentials";
                var body = UserEmails.Credentials(item.user.FullName, item.user.Email, item.tempPassword);

                await _email.SendAsync(item.user.PersonalEmail!, subject, body);

                item.rr.CredentialsEmailed = true;
                item.rr.TempPassword = null; // ✅ don’t return password if emailed
            }
            catch
            {
                // If email fails, still created user; return temp password so admin can deliver manually
                item.rr.CredentialsEmailed = false;
                item.rr.TempPassword = item.tempPassword;
                item.rr.Error = "User created but failed to send email (SMTP issue). Temp password returned.";

                _logger.LogError("Failed to send credentials email to {PersonalEmail} for user {Email}", item.user.PersonalEmail, item.user.Email);
            }
        }
        else
        {
            // no personal email, admin must deliver manually
            item.rr.CredentialsEmailed = false;
            item.rr.TempPassword = item.tempPassword;
        }

        result.Rows.Add(item.rr);
    }

    result.CreatedCount = toInsert.Count;
    result.FailedCount = result.TotalRows - result.CreatedCount;

    result.Rows = result.Rows.OrderBy(x => x.RowNumber).ToList();

    return new ResultDto<UserImportResultDto>
    {
        StatusCode = 200,
        IsSucced = true,
        Message = "Import completed.",
        Data = result
    };
}
    private static bool IsValidFinCode(string fin)
    {
        if (string.IsNullOrWhiteSpace(fin)) return false;

        fin = fin.Trim();

        if (fin.Length < 5 || fin.Length > 16) return false;

        // alphanumeric only
        return fin.All(char.IsLetterOrDigit);
    }

    public async Task<ResultDto<UserGetDto>> UpdateAsync(Guid id, UserUpdateDto dto)
{
    var user = await _users.GetAsync(x => x.Id == id);
    if (user is null)
        return new ResultDto<UserGetDto>
        {
            StatusCode = 404,
            IsSucced = false,
            Message = "User not found."
        };

    // Optional: prevent non-superadmin editing superadmin
    var callerRole = _http.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);
    var isCallerSuper = string.Equals(callerRole, "SuperAdmin", StringComparison.OrdinalIgnoreCase);

    if (user.Role == UserRole.SuperAdmin && !isCallerSuper)
        return new ResultDto<UserGetDto>
        {
            StatusCode = 403,
            IsSucced = false,
            Message = "Only SuperAdmin can update SuperAdmin user."
        };

    var personalEmail = string.IsNullOrWhiteSpace(dto.PersonalEmail)
        ? null
        : dto.PersonalEmail.Trim().ToLowerInvariant();

    if (!string.IsNullOrWhiteSpace(personalEmail) && !MailAddress.TryCreate(personalEmail, out _))
    {
        return new ResultDto<UserGetDto>
        {
            StatusCode = 400,
            IsSucced = false,
            Message = "PersonalEmail is not a valid email address."
        };
    }

    user.FullName = dto.FullName.Trim();
    user.PersonalEmail = personalEmail;
    user.PositionTitle = string.IsNullOrWhiteSpace(dto.PositionTitle) ? null : dto.PositionTitle.Trim();

    _users.Update(user);
    await _users.SaveChangesAsync();

    return new ResultDto<UserGetDto>
    {
        StatusCode = 200,
        IsSucced = true,
        Message = "User updated successfully.",
        Data = new UserGetDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            PersonalEmail = user.PersonalEmail,
            Role = user.Role,
            IsActive = user.IsActive,
            PositionTitle = user.PositionTitle
        }
    };
}

public async Task<ResultDto> DeleteAsync(Guid id)
{
    var currentId = CurrentUserId();
    if (currentId.HasValue && currentId.Value == id)
        return new ResultDto(400, false, "You cannot delete your own account.");

    var user = await _users.GetAsync(x => x.Id == id);
    if (user is null)
        return new ResultDto(404, false, "User not found.");

    // Optional: protect superadmin
    var callerRole = _http.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);
    var isCallerSuper = string.Equals(callerRole, "SuperAdmin", StringComparison.OrdinalIgnoreCase);

    if (user.Role == UserRole.SuperAdmin && !isCallerSuper)
        return new ResultDto(403, false, "Only SuperAdmin can delete SuperAdmin user.");

    // Soft delete via interceptor (mark as Deleted)
    _users.Delete(user);
    await _users.SaveChangesAsync();

    return new ResultDto(200, true, "User deleted successfully.");
}


}