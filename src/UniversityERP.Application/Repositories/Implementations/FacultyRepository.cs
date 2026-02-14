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
        var clean = name.Trim();

        var query = GetAll(ignoreQueryFilter)
            .AsNoTracking()
            .Where(x => x.Name == clean);

        if (excludeId.HasValue)
            query = query.Where(x => x.Id != excludeId.Value);

        return query.AnyAsync();
    }
}
