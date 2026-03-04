using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.TeacherDtos;

namespace UniversityERP.Infrastructure.Services.Abstractions;

public interface ITeacherService
{
    Task<ResultDto> CreateAsync(TeacherCreateDto dto);
    Task<ResultDto> UpdateAsync(TeacherUpdateDto dto);
    Task<ResultDto> DeleteAsync(Guid id);
    Task<ResultDto<List<TeacherGetDto>>> GetAllAsync();
    Task<ResultDto<TeacherGetDto>> GetByIdAsync(Guid id);
}