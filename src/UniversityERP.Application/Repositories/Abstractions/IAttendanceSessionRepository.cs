using UniversityERP.Application.Repositories.Abstractions.Generic;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Repositories.Abstractions;

public interface IAttendanceSessionRepository : IRepository<AttendanceSession>
{
    Task<bool> ExistsAsync(Guid courseOfferingId, DateOnly sessionDate, Guid? excludeId = null, bool ignoreQueryFilter = false);
}
