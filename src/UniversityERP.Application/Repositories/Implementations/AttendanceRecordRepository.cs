using Microsoft.EntityFrameworkCore;
using UniversityERP.Application.Contexts;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Application.Repositories.Implementations.Generic;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Repositories.Implementations;

internal class AttendanceRecordRepository(AppDbContext _context) : Repository<AttendanceRecord>(_context), IAttendanceRecordRepository
{
    public async Task<bool> ExistsAsync(Guid attendanceSessionId, Guid enrollmentCourseId, Guid? excludeId = null, bool ignoreQueryFilter = false)
    {
        var query = ignoreQueryFilter ? _context.AttendanceRecords.IgnoreQueryFilters() : _context.AttendanceRecords;

        return await query.AnyAsync(x =>
            x.AttendanceSessionId == attendanceSessionId &&
            x.EnrollmentCourseId == enrollmentCourseId &&
            (!excludeId.HasValue || x.Id != excludeId.Value));
    }
}
