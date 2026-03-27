using Microsoft.EntityFrameworkCore;
using UniversityERP.Application.Contexts;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Application.Repositories.Implementations.Generic;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Repositories.Implementations;

internal class ProgramCourseRepository(AppDbContext _context) : Repository<ProgramCourse>(_context), IProgramCourseRepository
{
    public async Task<bool> ExistsAsync(Guid academicProgramId, Guid academicCourseId, Guid? excludeId = null, bool ignoreQueryFilter = false)
    {
        var query = ignoreQueryFilter ? _context.ProgramCourses.IgnoreQueryFilters() : _context.ProgramCourses;

        return await query.AnyAsync(x =>
            x.AcademicProgramId == academicProgramId &&
            x.AcademicCourseId == academicCourseId &&
            (!excludeId.HasValue || x.Id != excludeId.Value));
    }
}
