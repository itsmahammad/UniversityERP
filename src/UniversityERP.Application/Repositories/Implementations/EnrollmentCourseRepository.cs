using Microsoft.EntityFrameworkCore;
using UniversityERP.Application.Contexts;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Application.Repositories.Implementations.Generic;
using UniversityERP.Domain.Entities;
using UniversityERP.Domain.Enums;

namespace UniversityERP.Application.Repositories.Implementations;

internal class EnrollmentCourseRepository(AppDbContext _context) : Repository<EnrollmentCourse>(_context), IEnrollmentCourseRepository
{
    public async Task<bool> ExistsAsync(Guid studentSemesterEnrollmentId, Guid courseOfferingId, Guid? excludeId = null, bool ignoreQueryFilter = false)
    {
        var query = ignoreQueryFilter ? _context.EnrollmentCourses.IgnoreQueryFilters() : _context.EnrollmentCourses;

        return await query.AnyAsync(x =>
            x.StudentSemesterEnrollmentId == studentSemesterEnrollmentId &&
            x.CourseOfferingId == courseOfferingId &&
            (!excludeId.HasValue || x.Id != excludeId.Value));
    }

    public async Task<int> GetTotalCreditsAsync(Guid studentSemesterEnrollmentId, bool ignoreQueryFilter = false)
    {
        var query = ignoreQueryFilter ? _context.EnrollmentCourses.IgnoreQueryFilters() : _context.EnrollmentCourses;

        return await query
            .Where(x => x.StudentSemesterEnrollmentId == studentSemesterEnrollmentId && x.Status != EnrollmentCourseStatus.Dropped)
            .SumAsync(x => (int?)x.CreditsSnapshot) ?? 0;
    }

    public async Task<int> CountAttemptsAsync(Guid studentId, Guid academicCourseId, bool ignoreQueryFilter = false)
    {
        var query = ignoreQueryFilter ? _context.EnrollmentCourses.IgnoreQueryFilters() : _context.EnrollmentCourses;

        return await query
            .Where(x =>
                x.StudentSemesterEnrollment.StudentId == studentId &&
                x.CourseOffering.AcademicCourseId == academicCourseId)
            .CountAsync();
    }

    public async Task<bool> HasCompletedCourseAsync(Guid studentId, Guid academicCourseId, DateOnly beforeSemesterStartDate, bool ignoreQueryFilter = false)
    {
        var query = ignoreQueryFilter ? _context.EnrollmentCourses.IgnoreQueryFilters() : _context.EnrollmentCourses;

        return await query.AnyAsync(x =>
            x.StudentSemesterEnrollment.StudentId == studentId &&
            x.CourseOffering.AcademicCourseId == academicCourseId &&
            x.Status == EnrollmentCourseStatus.Completed &&
            x.StudentSemesterEnrollment.Semester.StartDate < beforeSemesterStartDate);
    }
}
