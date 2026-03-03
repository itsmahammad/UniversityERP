using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.AcademicProgramDtos;

namespace UniversityERP.Infrastructure.Services.Abstractions;

public interface IAcademicProgramService
{
    Task<ResultDto> CreateAsync(AcademicProgramCreateDto dto);
    Task<ResultDto> UpdateAsync(AcademicProgramUpdateDto dto);
    Task<ResultDto> DeleteAsync(Guid id);
    Task<ResultDto<List<AcademicProgramGetDto>>> GetAllAsync();
    Task<ResultDto<AcademicProgramGetDto>> GetByIdAsync(Guid id);
}