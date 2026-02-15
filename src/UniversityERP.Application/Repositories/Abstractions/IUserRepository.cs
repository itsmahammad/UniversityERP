﻿using UniversityERP.Application.Repositories.Abstractions.Generic;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Repositories.Abstractions;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, bool ignoreQueryFilter = false);
    Task<bool> ExistsByEmailAsync(string email, Guid? excludeId = null, bool ignoreQueryFilter = false);
    Task<User?> GetByFinCodeAsync(string finCode, bool ignoreQueryFilter = false);
    Task<bool> ExistsByFinCodeAsync(string finCode, Guid? excludeId = null, bool ignoreQueryFilter = false);
}