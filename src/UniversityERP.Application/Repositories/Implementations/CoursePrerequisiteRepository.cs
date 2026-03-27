using Microsoft.EntityFrameworkCore;
using UniversityERP.Application.Contexts;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Application.Repositories.Implementations.Generic;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Repositories.Implementations;

internal class CoursePrerequisiteRepository(AppDbContext _context) : Repository<CoursePrerequisite>(_context), ICoursePrerequisiteRepository
{
    public async Task<bool> ExistsAsync(Guid academicCourseId, Guid prerequisiteAcademicCourseId, Guid? excludeId = null, bool ignoreQueryFilter = false)
    {
        var query = ignoreQueryFilter ? _context.CoursePrerequisites.IgnoreQueryFilters() : _context.CoursePrerequisites;

        return await query.AnyAsync(x =>
            x.AcademicCourseId == academicCourseId &&
            x.PrerequisiteAcademicCourseId == prerequisiteAcademicCourseId &&
            (!excludeId.HasValue || x.Id != excludeId.Value));
    }
}
