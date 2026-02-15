using System.Security.Claims;
using System.Security.Cryptography;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Domain.Entities;
using UniversityERP.Domain.Enums;
using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.Common;
using UniversityERP.Infrastructure.Dtos.UserDtos;
using UniversityERP.Infrastructure.Dtos.UserDtos.Import;
using UniversityERP.Infrastructure.Services.Abstractions;

namespace UniversityERP.Infrastructure.Services.Implementations;

internal class UserService : IUserService
{
    private readonly IUserRepository _users;
    private readonly IHttpContextAccessor _http;
    private readonly PasswordHasher<User> _hasher = new();

    public UserService(IUserRepository users, IHttpContextAccessor http)
    {
        _users = users;
        _http = http;
    }
    private Guid? CurrentUserId()
    {
        var sub = _http.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? _http.HttpContext?.User?.FindFirstValue("sub");

        return Guid.TryParse(sub, out var id) ? id : null;
    }

    public async Task<ResultDto<UserGetDto>> CreateAsync(UserCreateDto dto)
    {
        var email = dto.Email.Trim().ToLower();

        if (await _users.ExistsByEmailAsync(email, ignoreQueryFilter: true))
        {
            return new ResultDto<UserGetDto>
            {
                StatusCode = 409,
                IsSucced = false,
                Message = "Email already exists."
            };
        }

        var user = new User
        {
            FullName = dto.FullName.Trim(),
            Email = email,
            Role = dto.Role,
            IsActive = dto.IsActive,
            PositionTitle = string.IsNullOrWhiteSpace(dto.PositionTitle) ? null : dto.PositionTitle.Trim()
        };

        user.PasswordHash = _hasher.HashPassword(user, dto.Password);

        await _users.AddAsync(user);
        await _users.SaveChangesAsync();

        return new ResultDto<UserGetDto>
        {
            StatusCode = 201,
            IsSucced = true,
            Message = "User created successfully.",
            Data = new UserGetDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
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

    // Preload existing emails (ignore query filter so deleted accounts still block duplicates if you want)
    var existingEmails = await _users.GetAll(ignoreQueryFilter: true)
        .AsNoTracking()
        .Select(x => x.Email)
        .ToListAsync();

    var existingSet = new HashSet<string>(existingEmails, StringComparer.OrdinalIgnoreCase);
    var seenInFile = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

    var result = new UserImportResultDto { TotalRows = lastRow - 1 };

    var toInsert = new List<(User user, string tempPassword, UserImportRowResultDto rowResult)>();

    // Expected headers (row 1):
    // FullName | Email | Role | IsActive | PositionTitle
    for (int row = 2; row <= lastRow; row++)
    {
        var fullName = ws.Cell(row, 1).GetString()?.Trim() ?? "";
        var emailRaw = ws.Cell(row, 2).GetString()?.Trim() ?? "";
        var roleRaw = ws.Cell(row, 3).GetString()?.Trim() ?? "";
        var isActiveRaw = ws.Cell(row, 4).GetString()?.Trim() ?? "";
        var positionTitle = ws.Cell(row, 5).GetString()?.Trim();

        var rr = new UserImportRowResultDto
        {
            RowNumber = row,
            FullName = fullName,
            Email = emailRaw,
            Role = roleRaw,
            PositionTitle = string.IsNullOrWhiteSpace(positionTitle) ? null : positionTitle
        };

        if (string.IsNullOrWhiteSpace(fullName))
        {
            rr.Success = false;
            rr.Error = "FullName is required.";
            result.Rows.Add(rr);
            continue;
        }

        if (string.IsNullOrWhiteSpace(emailRaw))
        {
            rr.Success = false;
            rr.Error = "Email is required.";
            result.Rows.Add(rr);
            continue;
        }

        var email = emailRaw.Trim().ToLowerInvariant();

        if (!seenInFile.Add(email))
        {
            rr.Success = false;
            rr.Error = "Duplicate email in file.";
            result.Rows.Add(rr);
            continue;
        }

        if (existingSet.Contains(email))
        {
            rr.Success = false;
            rr.Error = "Email already exists in database.";
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

        var tempPassword = GenerateTempPassword();

        var user = new User
        {
            FullName = fullName,
            Email = email,
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

    // Fill success rows (with temp passwords)
    foreach (var item in toInsert)
    {
        item.rowResult.Success = true;
        item.rowResult.TempPassword = item.tempPassword;
        result.Rows.Add(item.rowResult);
    }

    result.CreatedCount = toInsert.Count;
    result.FailedCount = result.TotalRows - result.CreatedCount;

    // Sort rows by row number so response is readable
    result.Rows = result.Rows.OrderBy(x => x.RowNumber).ToList();

    return new ResultDto<UserImportResultDto>
    {
        StatusCode = 200,
        IsSucced = true,
        Message = "Import completed.",
        Data = result
    };
}



}