using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.CoursePrerequisiteDtos;

namespace UniversityERP.Infrastructure.Services.Abstractions;

public interface ICoursePrerequisiteService
{
    Task<ResultDto> CreateAsync(CoursePrerequisiteCreateDto dto);
    Task<ResultDto> UpdateAsync(CoursePrerequisiteUpdateDto dto);
    Task<ResultDto> DeleteAsync(Guid id);
    Task<ResultDto<List<CoursePrerequisiteGetDto>>> GetAllAsync();
    Task<ResultDto<CoursePrerequisiteGetDto>> GetByIdAsync(Guid id);
}
