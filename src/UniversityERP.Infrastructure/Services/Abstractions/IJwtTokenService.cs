using UniversityERP.Domain.Entities;

namespace UniversityERP.Infrastructure.Services.Abstractions;

public interface IJwtTokenService
{
    (string token, DateTime expiresAtUtc) CreateToken(User user);
}