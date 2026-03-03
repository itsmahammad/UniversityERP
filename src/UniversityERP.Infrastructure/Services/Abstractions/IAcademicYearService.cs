using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.AcademicYearDtos;

namespace UniversityERP.Infrastructure.Services.Abstractions;

public interface IAcademicYearService
{
    Task<ResultDto> CreateAsync(AcademicYearCreateDto dto);
    Task<ResultDto> UpdateAsync(AcademicYearUpdateDto dto);
    Task<ResultDto> DeleteAsync(Guid id);
    Task<ResultDto<List<AcademicYearGetDto>>> GetAllAsync();
    Task<ResultDto<AcademicYearGetDto>> GetByIdAsync(Guid id);
}