using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Domain.Entities;
using UniversityERP.Domain.Enums;
using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.Common;
using UniversityERP.Infrastructure.Dtos.UserDtos;
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


}