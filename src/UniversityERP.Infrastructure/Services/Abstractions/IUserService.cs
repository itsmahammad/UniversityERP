using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.UserDtos;

namespace UniversityERP.Infrastructure.Services.Abstractions;

public interface IUserService
{
    Task<ResultDto<UserGetDto>> CreateAsync(UserCreateDto dto);
}