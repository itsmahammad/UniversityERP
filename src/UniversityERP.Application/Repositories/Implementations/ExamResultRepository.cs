using Microsoft.EntityFrameworkCore;
using UniversityERP.Application.Contexts;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Application.Repositories.Implementations.Generic;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Repositories.Implementations;

internal class ExamResultRepository(AppDbContext _context) : Repository<ExamResult>(_context), IExamResultRepository
{
    public async Task<bool> ExistsAsync(Guid enrollmentCourseId, Guid examId, Guid? excludeId = null, bool ignoreQueryFilter = false)
    {
        var query = ignoreQueryFilter ? _context.ExamResults.IgnoreQueryFilters() : _context.ExamResults;

        return await query.AnyAsync(x =>
            x.EnrollmentCourseId == enrollmentCourseId &&
            x.ExamId == examId &&
            (!excludeId.HasValue || x.Id != excludeId.Value));
    }
}
