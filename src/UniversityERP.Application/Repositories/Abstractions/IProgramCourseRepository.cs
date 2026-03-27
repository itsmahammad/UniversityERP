using UniversityERP.Application.Repositories.Abstractions.Generic;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Repositories.Abstractions;

public interface IProgramCourseRepository : IRepository<ProgramCourse>
{
    Task<bool> ExistsAsync(Guid academicProgramId, Guid academicCourseId, Guid? excludeId = null, bool ignoreQueryFilter = false);
}
