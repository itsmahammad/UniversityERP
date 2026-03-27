using UniversityERP.Application.Repositories.Abstractions.Generic;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Repositories.Abstractions;

public interface ICoursePrerequisiteRepository : IRepository<CoursePrerequisite>
{
    Task<bool> ExistsAsync(Guid academicCourseId, Guid prerequisiteAcademicCourseId, Guid? excludeId = null, bool ignoreQueryFilter = false);
}
