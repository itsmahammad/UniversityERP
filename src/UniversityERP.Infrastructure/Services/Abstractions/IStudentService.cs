using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.StudentDtos;

namespace UniversityERP.Infrastructure.Services.Abstractions;

public interface IStudentService
{
    Task<ResultDto> CreateAsync(StudentCreateDto dto);
    Task<ResultDto> UpdateAsync(StudentUpdateDto dto);
    Task<ResultDto> DeleteAsync(Guid id);
    Task<ResultDto<List<StudentGetDto>>> GetAllAsync();
    Task<ResultDto<StudentGetDto>> GetByIdAsync(Guid id);
}