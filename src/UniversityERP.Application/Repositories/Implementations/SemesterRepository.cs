using Microsoft.EntityFrameworkCore;
using UniversityERP.Application.Contexts;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Application.Repositories.Implementations.Generic;
using UniversityERP.Domain.Entities;
using UniversityERP.Domain.Enums;

namespace UniversityERP.Application.Repositories.Implementations;

internal class SemesterRepository(AppDbContext _context) : Repository<Semester>(_context), ISemesterRepository
{
    public async Task<bool> ExistsTermAsync(Guid academicYearId, SemesterTerm term, Guid? excludeId = null, bool ignoreQueryFilter = false)
    {
        var query = ignoreQueryFilter ? _context.Semesters.IgnoreQueryFilters() : _context.Semesters;

        return await query.AnyAsync(x =>
            x.AcademicYearId == academicYearId &&
            x.Term == term &&
            (!excludeId.HasValue || x.Id != excludeId.Value));
    }
}