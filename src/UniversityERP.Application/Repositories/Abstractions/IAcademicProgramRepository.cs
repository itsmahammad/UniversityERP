using UniversityERP.Application.Repositories.Abstractions.Generic;
using UniversityERP.Domain.Entities;
using UniversityERP.Domain.Enums;

namespace UniversityERP.Application.Repositories.Abstractions;

public interface IAcademicProgramRepository : IRepository<AcademicProgram>
{
    Task<bool> ExistsByNameAsync(Guid departmentId, string name, ProgramLevel level, Guid? excludeId = null, bool ignoreQueryFilter = false);
}