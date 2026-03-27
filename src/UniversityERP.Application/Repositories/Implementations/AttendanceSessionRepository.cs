using Microsoft.EntityFrameworkCore;
using UniversityERP.Application.Contexts;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Application.Repositories.Implementations.Generic;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Repositories.Implementations;

internal class AttendanceSessionRepository(AppDbContext _context) : Repository<AttendanceSession>(_context), IAttendanceSessionRepository
{
    public async Task<bool> ExistsAsync(Guid courseOfferingId, DateOnly sessionDate, Guid? excludeId = null, bool ignoreQueryFilter = false)
    {
        var query = ignoreQueryFilter ? _context.AttendanceSessions.IgnoreQueryFilters() : _context.AttendanceSessions;

        return await query.AnyAsync(x =>
            x.CourseOfferingId == courseOfferingId &&
            x.SessionDate == sessionDate &&
            (!excludeId.HasValue || x.Id != excludeId.Value));
    }
}
