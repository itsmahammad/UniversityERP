﻿using UniversityERP.Domain.Enums;

namespace UniversityERP.Infrastructure.Dtos.UserDtos;

public class UserCreateDto
{
    public string FinCode { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public string? PersonalEmail { get; set; }

    public UserRole Role { get; set; }
    public bool IsActive { get; set; } = true;
    public string? PositionTitle { get; set; }
}