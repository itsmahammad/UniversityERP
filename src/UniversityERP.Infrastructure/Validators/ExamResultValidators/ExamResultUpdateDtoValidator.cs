using FluentValidation;
using UniversityERP.Infrastructure.Dtos.ExamResultDtos;

namespace UniversityERP.Infrastructure.Validators.ExamResultValidators;

public class ExamResultUpdateDtoValidator : AbstractValidator<ExamResultUpdateDto>
{
    public ExamResultUpdateDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ExamId).NotEmpty();
        RuleFor(x => x.NumericScore).GreaterThanOrEqualTo(0);
    }
}
