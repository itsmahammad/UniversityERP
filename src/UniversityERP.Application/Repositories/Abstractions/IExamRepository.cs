using UniversityERP.Application.Repositories.Abstractions.Generic;
using UniversityERP.Domain.Entities;
using UniversityERP.Domain.Enums;

namespace UniversityERP.Application.Repositories.Abstractions;

public interface IExamRepository : IRepository<Exam>
{
    Task<bool> ExistsAsync(Guid courseOfferingId, ExamType examType, Guid? excludeId = null, bool ignoreQueryFilter = false);
    Task<decimal> GetWeightSumAsync(Guid courseOfferingId, Guid? excludeId = null, bool ignoreQueryFilter = false);
}
