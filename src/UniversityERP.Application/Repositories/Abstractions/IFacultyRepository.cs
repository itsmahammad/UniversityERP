using UniversityERP.Application.Repositories.Abstractions.Generic;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Repositories.Abstractions;

public interface IFacultyRepository : IRepository<Faculty>
{
    Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null, bool ignoreQueryFilter = false);
}
