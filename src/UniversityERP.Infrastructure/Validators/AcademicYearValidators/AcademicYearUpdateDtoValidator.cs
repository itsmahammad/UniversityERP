using FluentValidation;
using UniversityERP.Infrastructure.Dtos.AcademicYearDtos;

namespace UniversityERP.Infrastructure.Validators.AcademicYearValidators;

public class AcademicYearUpdateDtoValidator : AbstractValidator<AcademicYearUpdateDto>
{
    public AcademicYearUpdateDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(30);
        RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate);
    }
}