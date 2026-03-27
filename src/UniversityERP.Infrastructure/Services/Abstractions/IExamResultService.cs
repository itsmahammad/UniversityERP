using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.ExamResultDtos;

namespace UniversityERP.Infrastructure.Services.Abstractions;

public interface IExamResultService
{
    Task<ResultDto> CreateAsync(Guid enrollmentCourseId, ExamResultCreateDto dto);
    Task<ResultDto> UpdateAsync(Guid enrollmentCourseId, ExamResultUpdateDto dto);
    Task<ResultDto> DeleteAsync(Guid enrollmentCourseId, Guid id);
    Task<ResultDto<List<ExamResultGetDto>>> GetAllAsync(Guid enrollmentCourseId);
    Task<ResultDto<ExamResultGetDto>> GetByIdAsync(Guid enrollmentCourseId, Guid id);
}
