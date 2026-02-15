﻿using Microsoft.AspNetCore.Identity;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Domain.Entities;
using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.AuthDtos;
using UniversityERP.Infrastructure.Services.Abstractions;

namespace UniversityERP.Infrastructure.Services.Implementations;

internal class AuthService : IAuthService
{
    private readonly IUserRepository _users;
    private readonly IJwtTokenService _jwt;
    private readonly PasswordHasher<User> _hasher = new();
    public AuthService(IUserRepository users, IJwtTokenService jwt)
    {
        _users = users;
        _jwt = jwt;
    }

    public async Task<ResultDto<LoginResponseDto>> LoginAsync(LoginDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            return new ResultDto<LoginResponseDto>
            {
                StatusCode = 400,
                IsSucced = false,
                Message = "Email and password are required."
            };

        User? user = null;

        // If identifier contains '@', treat as email; otherwise treat as FinCode
        if (dto.Email.Contains('@'))
        {
            user = await _users.GetByEmailAsync(dto.Email);
        }
        else
        {
            user = await _users.GetByFinCodeAsync(dto.Email);
        }

        if (user is null || !user.IsActive)
            return new ResultDto<LoginResponseDto>
            {
                StatusCode = 401,
                IsSucced = false,
                Message = "Invalid credentials."
            };

        var verify = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
        if (verify == PasswordVerificationResult.Failed)
            return new ResultDto<LoginResponseDto>
            {
                StatusCode = 401,
                IsSucced = false,
                Message = "Invalid credentials."
            };

        var (token, exp) = _jwt.CreateToken(user);

        return new ResultDto<LoginResponseDto>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = new LoginResponseDto
            {
                AccessToken = token,
                ExpiresAtUtc = exp,
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.ToString()
            }
        };
    }
}
