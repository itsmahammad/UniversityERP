using Microsoft.EntityFrameworkCore;
using UniversityERP.Application.Contexts;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Application.Repositories.Implementations.Generic;
using UniversityERP.Domain.Entities;
using UniversityERP.Domain.Enums;

namespace UniversityERP.Application.Repositories.Implementations;

internal class ExamRepository(AppDbContext _context) : Repository<Exam>(_context), IExamRepository
{
    public async Task<bool> ExistsAsync(Guid courseOfferingId, ExamType examType, Guid? excludeId = null, bool ignoreQueryFilter = false)
    {
        var query = ignoreQueryFilter ? _context.Exams.IgnoreQueryFilters() : _context.Exams;

        return await query.AnyAsync(x =>
            x.CourseOfferingId == courseOfferingId &&
            x.ExamType == examType &&
            (!excludeId.HasValue || x.Id != excludeId.Value));
    }

    public async Task<decimal> GetWeightSumAsync(Guid courseOfferingId, Guid? excludeId = null, bool ignoreQueryFilter = false)
    {
        var query = ignoreQueryFilter ? _context.Exams.IgnoreQueryFilters() : _context.Exams;

        return await query
            .Where(x =>
                x.CourseOfferingId == courseOfferingId &&
                (!excludeId.HasValue || x.Id != excludeId.Value))
            .SumAsync(x => (decimal?)x.WeightPercentage) ?? 0m;
    }
}
