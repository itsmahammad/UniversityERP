using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.ExamDtos;

namespace UniversityERP.Infrastructure.Services.Abstractions;

public interface IExamService
{
    Task<ResultDto> CreateAsync(ExamCreateDto dto);
    Task<ResultDto> UpdateAsync(ExamUpdateDto dto);
    Task<ResultDto> DeleteAsync(Guid id);
    Task<ResultDto<List<ExamGetDto>>> GetAllAsync();
    Task<ResultDto<ExamGetDto>> GetByIdAsync(Guid id);
}
