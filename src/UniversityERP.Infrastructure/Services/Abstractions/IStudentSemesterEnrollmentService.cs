using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.StudentSemesterEnrollmentDtos;

namespace UniversityERP.Infrastructure.Services.Abstractions;

public interface IStudentSemesterEnrollmentService
{
    Task<ResultDto> CreateAsync(StudentSemesterEnrollmentCreateDto dto);
    Task<ResultDto> DeleteAsync(Guid id);
    Task<ResultDto> SubmitAsync(Guid id);
    Task<ResultDto<List<StudentSemesterEnrollmentGetDto>>> GetAllAsync();
    Task<ResultDto<StudentSemesterEnrollmentGetDto>> GetByIdAsync(Guid id);
}
