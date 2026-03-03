using UniversityERP.Application.Repositories.Abstractions.Generic;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Repositories.Abstractions;

public interface IDepartmentRepository : IRepository<Department>
{
    Task<bool> ExistsByNameAsync(Guid facultyId, string name, Guid? excludeId = null, bool ignoreQueryFilter = false);
    Task<bool> ExistsByCodeAsync(Guid facultyId, string code, Guid? excludeId = null, bool ignoreQueryFilter = false);
}