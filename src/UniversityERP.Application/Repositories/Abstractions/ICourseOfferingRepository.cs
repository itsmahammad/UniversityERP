using UniversityERP.Application.Repositories.Abstractions.Generic;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Repositories.Abstractions;

public interface ICourseOfferingRepository : IRepository<CourseOffering>
{
    Task<bool> ExistsAsync(Guid academicCourseId, Guid semesterId, Guid teacherId, string section, Guid? excludeId = null, bool ignoreQueryFilter = false);
}
