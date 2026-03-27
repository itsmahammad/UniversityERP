namespace UniversityERP.Application.Repositories.Abstractions;

public static class CourseOfferingSectionNormalizer
{
    public static string Normalize(string? section)
        => string.IsNullOrWhiteSpace(section) ? string.Empty : section.Trim().ToUpperInvariant();
}
