using FluentValidation;
using UniversityERP.Infrastructure.Dtos.AcademicYearDtos;

namespace UniversityERP.Infrastructure.Validators.AcademicYearValidators;

public class AcademicYearCreateDtoValidator : AbstractValidator<AcademicYearCreateDto>
{
    public AcademicYearCreateDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(30);
        RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate);
    }
}