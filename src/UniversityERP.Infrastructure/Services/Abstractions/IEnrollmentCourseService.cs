using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.EnrollmentCourseDtos;

namespace UniversityERP.Infrastructure.Services.Abstractions;

public interface IEnrollmentCourseService
{
    Task<ResultDto> CreateAsync(Guid studentSemesterEnrollmentId, EnrollmentCourseCreateDto dto);
    Task<ResultDto> DeleteAsync(Guid studentSemesterEnrollmentId, Guid id);
    Task<ResultDto<List<EnrollmentCourseGetDto>>> GetAllAsync(Guid studentSemesterEnrollmentId);
    Task<ResultDto<EnrollmentCourseGetDto>> GetByIdAsync(Guid studentSemesterEnrollmentId, Guid id);
}
