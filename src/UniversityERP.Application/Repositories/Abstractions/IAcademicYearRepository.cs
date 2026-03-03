using UniversityERP.Application.Repositories.Abstractions.Generic;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Repositories.Abstractions;

public interface IAcademicYearRepository : IRepository<AcademicYear>
{
    Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null, bool ignoreQueryFilter = false);
}