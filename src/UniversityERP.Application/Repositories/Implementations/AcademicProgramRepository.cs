using Microsoft.EntityFrameworkCore;
using UniversityERP.Application.Contexts;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Application.Repositories.Implementations.Generic;
using UniversityERP.Domain.Entities;
using UniversityERP.Domain.Enums;

namespace UniversityERP.Application.Repositories.Implementations;

internal class AcademicProgramRepository(AppDbContext _context) : Repository<AcademicProgram>(_context), IAcademicProgramRepository
{
    public async Task<bool> ExistsByNameAsync(Guid departmentId, string name, ProgramLevel level, Guid? excludeId = null,
        bool ignoreQueryFilter = false)
    {
        var query = ignoreQueryFilter ? _context.AcademicPrograms.IgnoreQueryFilters() : _context.AcademicPrograms;

        name = name.Trim().ToLowerInvariant();

        return await query.AnyAsync(x =>
            x.DepartmentId == departmentId &&
            x.Level == level &&
            x.Name.ToLower() == name &&
            (!excludeId.HasValue || x.Id != excludeId.Value));
    }
}