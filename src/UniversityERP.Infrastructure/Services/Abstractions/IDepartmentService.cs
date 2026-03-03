using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.DepartmentDtos;

namespace UniversityERP.Infrastructure.Services.Abstractions;

public interface IDepartmentService
{
    Task<ResultDto> CreateAsync(DepartmentCreateDto dto);
    Task<ResultDto> UpdateAsync(DepartmentUpdateDto dto);
    Task<ResultDto> DeleteAsync(Guid id);
    Task<ResultDto<List<DepartmentGetDto>>> GetAllAsync();
    Task<ResultDto<DepartmentGetDto>> GetByIdAsync(Guid id);
}