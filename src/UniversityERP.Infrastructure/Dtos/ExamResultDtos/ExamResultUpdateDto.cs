namespace UniversityERP.Infrastructure.Dtos.ExamResultDtos;

public class ExamResultUpdateDto
{
    public Guid Id { get; set; }
    public Guid ExamId { get; set; }
    public decimal NumericScore { get; set; }
}
