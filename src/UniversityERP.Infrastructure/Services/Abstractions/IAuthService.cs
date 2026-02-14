using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.AuthDtos;

namespace UniversityERP.Infrastructure.Services.Abstractions;

public interface IAuthService
{
    Task<ResultDto<LoginResponseDto>> LoginAsync(LoginDto dto);
}