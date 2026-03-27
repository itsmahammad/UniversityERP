using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.CourseOfferingDtos;

namespace UniversityERP.Infrastructure.Services.Abstractions;

public interface ICourseOfferingService
{
    Task<ResultDto> CreateAsync(CourseOfferingCreateDto dto);
    Task<ResultDto> UpdateAsync(CourseOfferingUpdateDto dto);
    Task<ResultDto> DeleteAsync(Guid id);
    Task<ResultDto<List<CourseOfferingGetDto>>> GetAllAsync();
    Task<ResultDto<CourseOfferingGetDto>> GetByIdAsync(Guid id);
}
