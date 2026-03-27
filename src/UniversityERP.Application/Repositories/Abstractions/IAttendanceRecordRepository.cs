using UniversityERP.Application.Repositories.Abstractions.Generic;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Repositories.Abstractions;

public interface IAttendanceRecordRepository : IRepository<AttendanceRecord>
{
    Task<bool> ExistsAsync(Guid attendanceSessionId, Guid enrollmentCourseId, Guid? excludeId = null, bool ignoreQueryFilter = false);
}
