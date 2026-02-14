using Microsoft.AspNetCore.Identity;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Domain.Entities;
using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.UserDtos;
using UniversityERP.Infrastructure.Services.Abstractions;

namespace UniversityERP.Infrastructure.Services.Implementations;

internal class UserService : IUserService
{
    private readonly IUserRepository _users;
    private readonly PasswordHasher<User> _hasher = new();

    public UserService(IUserRepository users)
    {
        _users = users;
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
}