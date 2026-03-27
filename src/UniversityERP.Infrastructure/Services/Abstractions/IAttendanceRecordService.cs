using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.AttendanceRecordDtos;

namespace UniversityERP.Infrastructure.Services.Abstractions;

public interface IAttendanceRecordService
{
    Task<ResultDto> CreateAsync(Guid attendanceSessionId, AttendanceRecordCreateDto dto);
    Task<ResultDto> UpdateAsync(Guid attendanceSessionId, AttendanceRecordUpdateDto dto);
    Task<ResultDto> DeleteAsync(Guid attendanceSessionId, Guid id);
    Task<ResultDto<List<AttendanceRecordGetDto>>> GetAllAsync(Guid attendanceSessionId);
    Task<ResultDto<AttendanceRecordGetDto>> GetByIdAsync(Guid attendanceSessionId, Guid id);
}
