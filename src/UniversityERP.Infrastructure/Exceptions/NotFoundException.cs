using UniversityERP.Infrastructure.Abstractions;

namespace UniversityERP.Infrastructure.Exceptions;

public class NotFoundException(string message = "Object not found", int statusCode = 404) : Exception(message), IBaseException
{
    public int StatusCode { get; set; } = statusCode;
}
