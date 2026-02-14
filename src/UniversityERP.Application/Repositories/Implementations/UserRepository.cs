using Microsoft.EntityFrameworkCore;
using UniversityERP.Application.Contexts;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Application.Repositories.Implementations.Generic;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Repositories.Implementations;

internal class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context) { }

    public Task<User?> GetByEmailAsync(string email, bool ignoreQueryFilter = false)
    {
        var e = email.Trim().ToLower();
        return GetAll(ignoreQueryFilter)
            .FirstOrDefaultAsync(x => x.Email.ToLower() == e);
    }

    public Task<bool> ExistsByEmailAsync(string email, Guid? excludeId = null, bool ignoreQueryFilter = false)
    {
        var e = email.Trim().ToLower();

        var q = GetAll(ignoreQueryFilter)
            .AsNoTracking()
            .Where(x => x.Email.ToLower() == e);

        if (excludeId.HasValue)
            q = q.Where(x => x.Id != excludeId.Value);

        return q.AnyAsync();
    }
}