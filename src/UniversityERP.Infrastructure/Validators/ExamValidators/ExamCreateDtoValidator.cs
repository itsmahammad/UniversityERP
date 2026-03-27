using FluentValidation;
using UniversityERP.Infrastructure.Dtos.ExamDtos;

namespace UniversityERP.Infrastructure.Validators.ExamValidators;

public class ExamCreateDtoValidator : AbstractValidator<ExamCreateDto>
{
    public ExamCreateDtoValidator()
    {
        RuleFor(x => x.CourseOfferingId).NotEmpty();
        RuleFor(x => x.MaxScore).GreaterThan(0);
        RuleFor(x => x.WeightPercentage).GreaterThan(0).LessThanOrEqualTo(100);
    }
}
