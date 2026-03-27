using Microsoft.EntityFrameworkCore;
using UniversityERP.Application.Contexts;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Application.Repositories.Implementations.Generic;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Repositories.Implementations;

internal class CourseOfferingRepository(AppDbContext _context) : Repository<CourseOffering>(_context), ICourseOfferingRepository
{
    public async Task<bool> ExistsAsync(Guid academicCourseId, Guid semesterId, Guid teacherId, string section, Guid? excludeId = null, bool ignoreQueryFilter = false)
    {
        var query = ignoreQueryFilter ? _context.CourseOfferings.IgnoreQueryFilters() : _context.CourseOfferings;
        section = CourseOfferingSectionNormalizer.Normalize(section);

        return await query.AnyAsync(x =>
            x.AcademicCourseId == academicCourseId &&
            x.SemesterId == semesterId &&
            x.TeacherId == teacherId &&
            x.Section == section &&
            (!excludeId.HasValue || x.Id != excludeId.Value));
    }
}
