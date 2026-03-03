using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.SemesterDtos;

namespace UniversityERP.Infrastructure.Services.Abstractions;

public interface ISemesterService
{
    Task<ResultDto> CreateAsync(SemesterCreateDto dto);
    Task<ResultDto> UpdateAsync(SemesterUpdateDto dto);
    Task<ResultDto> DeleteAsync(Guid id);
    Task<ResultDto<List<SemesterGetDto>>> GetAllAsync();
    Task<ResultDto<SemesterGetDto>> GetByIdAsync(Guid id);
}