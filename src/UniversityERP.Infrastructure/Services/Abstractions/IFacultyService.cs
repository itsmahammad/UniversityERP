using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.FacultyDtos;

namespace UniversityERP.Infrastructure.Services.Abstractions;

public interface IFacultyService
{
    Task<ResultDto> CreateAsync(FacultyCreateDto dto);
    Task<ResultDto> UpdateAsync(FacultyUpdateDto dto);
    Task<ResultDto> DeleteAsync(Guid id);
    Task<ResultDto<List<FacultyGetDto>>> GetAllAsync();
    Task<ResultDto<FacultyGetDto>> GetByIdAsync(Guid id);
}