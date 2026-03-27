using UniversityERP.Application.Repositories.Abstractions.Generic;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Repositories.Abstractions;

public interface IStudentSemesterEnrollmentRepository : IRepository<StudentSemesterEnrollment>
{
    Task<bool> ExistsAsync(Guid studentId, Guid semesterId, Guid? excludeId = null, bool ignoreQueryFilter = false);
}
