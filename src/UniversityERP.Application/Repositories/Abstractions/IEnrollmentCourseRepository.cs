using UniversityERP.Application.Repositories.Abstractions.Generic;
using UniversityERP.Domain.Entities;
using UniversityERP.Domain.Enums;

namespace UniversityERP.Application.Repositories.Abstractions;

public interface IEnrollmentCourseRepository : IRepository<EnrollmentCourse>
{
    Task<bool> ExistsAsync(Guid studentSemesterEnrollmentId, Guid courseOfferingId, Guid? excludeId = null, bool ignoreQueryFilter = false);
    Task<int> GetTotalCreditsAsync(Guid studentSemesterEnrollmentId, bool ignoreQueryFilter = false);
    Task<int> CountAttemptsAsync(Guid studentId, Guid academicCourseId, bool ignoreQueryFilter = false);
    Task<bool> HasCompletedCourseAsync(Guid studentId, Guid academicCourseId, DateOnly beforeSemesterStartDate, bool ignoreQueryFilter = false);
}
