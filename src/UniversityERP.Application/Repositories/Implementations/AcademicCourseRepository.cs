using Microsoft.EntityFrameworkCore;
using UniversityERP.Application.Contexts;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Application.Repositories.Implementations.Generic;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Repositories.Implementations;

internal class AcademicCourseRepository : Repository<AcademicCourse>, IAcademicCourseRepository
{
    private readonly AppDbContext _context;

    public AcademicCourseRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> ExistsByCodeAsync(string code, Guid? excludeId = null, bool ignoreQueryFilter = false)
    {
        var query = ignoreQueryFilter ? _context.AcademicCourses.IgnoreQueryFilters() : _context.AcademicCourses;
        code = code.Trim().ToLowerInvariant();

        return await query.AnyAsync(x =>
            x.Code.ToLower() == code &&
            (!excludeId.HasValue || x.Id != excludeId.Value));
    }

    public async Task<bool> ExistsByNameAsync(Guid owningDepartmentId, string name, Guid? excludeId = null, bool ignoreQueryFilter = false)
    {
        var query = ignoreQueryFilter ? _context.AcademicCourses.IgnoreQueryFilters() : _context.AcademicCourses;
        name = name.Trim().ToLowerInvariant();

        return await query.AnyAsync(x =>
            x.OwningDepartmentId == owningDepartmentId &&
            x.Name.ToLower() == name &&
            (!excludeId.HasValue || x.Id != excludeId.Value));
    }
}