using UniversityERP.Application.Repositories.Abstractions.Generic;
using UniversityERP.Domain.Entities;

namespace UniversityERP.Application.Repositories.Abstractions;

public interface IStudentRepository : IRepository<Student>
{
    Task<bool> ExistsByUserIdAsync(Guid userId, Guid? excludeId = null, bool ignoreQueryFilter = false);
}