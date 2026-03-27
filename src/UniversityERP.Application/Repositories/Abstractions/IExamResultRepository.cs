using UniversityERP.Application.Repositories.Abstractions.Generic;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Repositories.Abstractions;

public interface IExamResultRepository : IRepository<ExamResult>
{
    Task<bool> ExistsAsync(Guid enrollmentCourseId, Guid examId, Guid? excludeId = null, bool ignoreQueryFilter = false);
}
