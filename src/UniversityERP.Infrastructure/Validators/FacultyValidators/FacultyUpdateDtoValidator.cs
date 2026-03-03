using FluentValidation;
using UniversityERP.Infrastructure.Dtos.FacultyDtos;

namespace UniversityERP.Infrastructure.Validators.FacultyValidators;

public class FacultyUpdateDtoValidator : AbstractValidator<FacultyUpdateDto>
{
    public FacultyUpdateDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(20)
            .Matches("^[A-Za-z0-9]+$").WithMessage("Code must be alphanumeric with no spaces.");
    }
}