using UniversityERP.Application.Repositories.Abstractions.Generic;
using UniversityERP.Domain.Entities;
using UniversityERP.Domain.Enums;

namespace UniversityERP.Application.Repositories.Abstractions;

public interface ISemesterRepository : IRepository<Semester>
{
    Task<bool> ExistsTermAsync(Guid academicYearId, SemesterTerm term, Guid? excludeId = null, bool ignoreQueryFilter = false);
}