using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.ProgramCourseDtos;

namespace UniversityERP.Infrastructure.Services.Abstractions;

public interface IProgramCourseService
{
    Task<ResultDto> CreateAsync(ProgramCourseCreateDto dto);
    Task<ResultDto> UpdateAsync(ProgramCourseUpdateDto dto);
    Task<ResultDto> DeleteAsync(Guid id);
    Task<ResultDto<List<ProgramCourseGetDto>>> GetAllAsync();
    Task<ResultDto<ProgramCourseGetDto>> GetByIdAsync(Guid id);
}
