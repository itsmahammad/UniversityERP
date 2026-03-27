using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.GpaDtos;

namespace UniversityERP.Infrastructure.Services.Abstractions;

public interface IGpaService
{
    Task<ResultDto<SemesterGpaDto>> GetSemesterGpaAsync(Guid studentSemesterEnrollmentId);
    Task<ResultDto<CumulativeGpaDto>> GetCumulativeGpaAsync(Guid studentId);
}
