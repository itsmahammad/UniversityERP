using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.AttendanceSessionDtos;

namespace UniversityERP.Infrastructure.Services.Abstractions;

public interface IAttendanceSessionService
{
    Task<ResultDto> CreateAsync(AttendanceSessionCreateDto dto);
    Task<ResultDto> UpdateAsync(AttendanceSessionUpdateDto dto);
    Task<ResultDto> DeleteAsync(Guid id);
    Task<ResultDto<List<AttendanceSessionGetDto>>> GetAllAsync();
    Task<ResultDto<AttendanceSessionGetDto>> GetByIdAsync(Guid id);
}
