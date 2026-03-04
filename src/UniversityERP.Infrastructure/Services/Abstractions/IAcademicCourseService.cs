using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.AcademicCourseDtos;

namespace UniversityERP.Infrastructure.Services.Abstractions;

public interface IAcademicCourseService
{
    Task<ResultDto> CreateAsync(AcademicCourseCreateDto dto);
    Task<ResultDto> UpdateAsync(AcademicCourseUpdateDto dto);
    Task<ResultDto> DeleteAsync(Guid id);
    Task<ResultDto<List<AcademicCourseGetDto>>> GetAllAsync();
    Task<ResultDto<AcademicCourseGetDto>> GetByIdAsync(Guid id);
}