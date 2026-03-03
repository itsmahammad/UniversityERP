using Microsoft.EntityFrameworkCore;
using UniversityERP.Application.Contexts;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Application.Repositories.Implementations.Generic;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Repositories.Implementations;

internal class FacultyRepository(AppDbContext _context) : Repository<Faculty>(_context), IFacultyRepository
{
    public Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null, bool ignoreQueryFilter = false)
    {
        var clean = name.Trim().ToLowerInvariant();

        var query = GetAll(ignoreQueryFilter)
            .AsNoTracking()
            .Where(x => x.Name.ToLower() == clean);

        if (excludeId.HasValue)
            query = query.Where(x => x.Id != excludeId.Value);

        return query.AnyAsync();
    }

    public Task<bool> ExistsByCodeAsync(string code, Guid? excludeId = null, bool ignoreQueryFilter = false)
    {
        var clean = code.Trim().ToLowerInvariant();

        var query = GetAll(ignoreQueryFilter)
            .AsNoTracking()
            .Where(x => x.Code.ToLower() == clean);

        if (excludeId.HasValue)
            query = query.Where(x => x.Id != excludeId.Value);

        return query.AnyAsync();
    }
}