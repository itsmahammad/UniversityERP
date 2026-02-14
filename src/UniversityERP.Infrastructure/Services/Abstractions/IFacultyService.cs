using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.FacultyDtos;

namespace UniversityERP.Infrastructure.Services.Abstractions;

public interface IFacultyService
{
    Task<ResultDto<List<FacultyGetDto>>> GetAllAsync();
    Task<ResultDto<FacultyGetDto>> GetByIdAsync(Guid id);
    Task<ResultDto> CreateAsync(FacultyCreateDto dto);

    Task<ResultDto> UpdateAsync(Guid id, FacultyUpdateDto dto);
    Task<ResultDto> DeleteAsync(Guid id);
}
