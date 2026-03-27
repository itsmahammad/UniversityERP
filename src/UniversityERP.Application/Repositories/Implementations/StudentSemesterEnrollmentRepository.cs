using Microsoft.EntityFrameworkCore;
using UniversityERP.Application.Contexts;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Application.Repositories.Implementations.Generic;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Repositories.Implementations;

internal class StudentSemesterEnrollmentRepository(AppDbContext _context) : Repository<StudentSemesterEnrollment>(_context), IStudentSemesterEnrollmentRepository
{
    public async Task<bool> ExistsAsync(Guid studentId, Guid semesterId, Guid? excludeId = null, bool ignoreQueryFilter = false)
    {
        var query = ignoreQueryFilter ? _context.StudentSemesterEnrollments.IgnoreQueryFilters() : _context.StudentSemesterEnrollments;

        return await query.AnyAsync(x =>
            x.StudentId == studentId &&
            x.SemesterId == semesterId &&
            (!excludeId.HasValue || x.Id != excludeId.Value));
    }
}
