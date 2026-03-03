using FluentValidation;
using UniversityERP.Infrastructure.Dtos.DepartmentDtos;

namespace UniversityERP.Infrastructure.Validators.DepartmentValidators;

public class DepartmentUpdateDtoValidator : AbstractValidator<DepartmentUpdateDto>
{
    public DepartmentUpdateDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.FacultyId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(30)
            .Matches("^[A-Za-z0-9]+$").WithMessage("Code must be alphanumeric with no spaces.");
    }
}