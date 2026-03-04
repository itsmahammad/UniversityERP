using Microsoft.EntityFrameworkCore;
using UniversityERP.Application.Contexts;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Application.Repositories.Implementations.Generic;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Repositories.Implementations;

internal class TeacherRepository : Repository<Teacher>, ITeacherRepository
{
    private readonly AppDbContext _context;

    public TeacherRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> ExistsByUserIdAsync(Guid userId, Guid? excludeId = null, bool ignoreQueryFilter = false)
    {
        var query = ignoreQueryFilter ? _context.Teachers.IgnoreQueryFilters() : _context.Teachers;

        return await query.AnyAsync(x =>
            x.UserId == userId &&
            (!excludeId.HasValue || x.Id != excludeId.Value));
    }
}