using Microsoft.EntityFrameworkCore;
using UniversityERP.Application.Contexts;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Application.Repositories.Implementations.Generic;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Repositories.Implementations;

internal class AcademicYearRepository(AppDbContext _context) : Repository<AcademicYear>(_context), IAcademicYearRepository
{
    public async Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null, bool ignoreQueryFilter = false)
    {
        var query = ignoreQueryFilter ? _context.AcademicYears.IgnoreQueryFilters() : _context.AcademicYears;

        name = name.Trim().ToLowerInvariant();

        return await query.AnyAsync(x =>
            x.Name.ToLower() == name &&
            (!excludeId.HasValue || x.Id != excludeId.Value));
    }
}