namespace UniversityERP.Infrastructure.Services.Implementations;

internal static class GradeScaleHelper
{
    public static (string LetterGrade, decimal GradePoint) ToGrade(decimal numericScore)
    {
        if (numericScore >= 93m) return ("A", 4.0m);
        if (numericScore >= 90m) return ("A-", 3.7m);
        if (numericScore >= 87m) return ("B+", 3.3m);
        if (numericScore >= 83m) return ("B", 3.0m);
        if (numericScore >= 80m) return ("B-", 2.7m);
        if (numericScore >= 77m) return ("C+", 2.3m);
        if (numericScore >= 73m) return ("C", 2.0m);
        if (numericScore >= 70m) return ("C-", 1.7m);
        if (numericScore >= 67m) return ("D+", 1.3m);
        if (numericScore >= 60m) return ("D", 1.0m);
        return ("F", 0m);
    }
}
