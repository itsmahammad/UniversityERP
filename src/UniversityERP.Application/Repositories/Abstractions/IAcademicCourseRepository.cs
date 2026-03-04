using UniversityERP.Application.Repositories.Abstractions.Generic;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Repositories.Abstractions;

public interface IAcademicCourseRepository : IRepository<AcademicCourse>
{
    Task<bool> ExistsByCodeAsync(string code, Guid? excludeId = null, bool ignoreQueryFilter = false);
    Task<bool> ExistsByNameAsync(Guid owningDepartmentId, string name, Guid? excludeId = null, bool ignoreQueryFilter = false);
}