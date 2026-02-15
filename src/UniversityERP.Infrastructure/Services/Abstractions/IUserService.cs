using UniversityERP.Domain.Enums;
using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.Common;
using UniversityERP.Infrastructure.Dtos.UserDtos;

namespace UniversityERP.Infrastructure.Services.Abstractions;

public interface IUserService
{
    Task<ResultDto<UserGetDto>> CreateAsync(UserCreateDto dto);

    Task<ResultDto<PagedResponseDto<UserGetDto>>> GetPagedAsync(
        int page,
        int pageSize,
        string? search,
        UserRole? role,
        bool? isActive);

    Task<ResultDto<UserGetDto>> GetByIdAsync(Guid id);

    Task<ResultDto> ActivateAsync(Guid id);
    Task<ResultDto> DeactivateAsync(Guid id);

    Task<ResultDto> ChangeRoleAsync(Guid id, ChangeRoleDto dto);

    Task<ResultDto> ResetPasswordAsync(Guid id, ResetPasswordDto dto);

    Task<ResultDto<UserGetDto>> GetMeAsync();
    Task<ResultDto> ChangeMyPasswordAsync(ChangePasswordDto dto);

}