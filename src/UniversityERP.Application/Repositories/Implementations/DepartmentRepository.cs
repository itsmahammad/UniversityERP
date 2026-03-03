using Microsoft.EntityFrameworkCore;
using UniversityERP.Application.Contexts;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Application.Repositories.Implementations.Generic;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Repositories.Implementations;

internal class DepartmentRepository(AppDbContext _context) : Repository<Department>(_context), IDepartmentRepository
{
    public async Task<bool> ExistsByNameAsync(Guid facultyId, string name, Guid? excludeId = null, bool ignoreQueryFilter = false)
    {
        var query = ignoreQueryFilter ? _context.Departments.IgnoreQueryFilters() : _context.Departments;

        name = name.Trim().ToLowerInvariant();

        return await query.AnyAsync(x =>
            x.FacultyId == facultyId &&
            x.Name.ToLower() == name &&
            (!excludeId.HasValue || x.Id != excludeId.Value));
    }

    public async Task<bool> ExistsByCodeAsync(Guid facultyId, string code, Guid? excludeId = null, bool ignoreQueryFilter = false)
    {
        var query = ignoreQueryFilter ? _context.Departments.IgnoreQueryFilters() : _context.Departments;

        code = code.Trim().ToLowerInvariant();

        return await query.AnyAsync(x =>
            x.FacultyId == facultyId &&
            x.Code.ToLower() == code &&
            (!excludeId.HasValue || x.Id != excludeId.Value));
    }
}