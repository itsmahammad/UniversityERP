using FluentValidation;
using UniversityERP.Infrastructure.Dtos.AcademicProgramDtos;

namespace UniversityERP.Infrastructure.Validators.AcademicProgramValidators;

public class AcademicProgramUpdateDtoValidator : AbstractValidator<AcademicProgramUpdateDto>
{
    public AcademicProgramUpdateDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.DepartmentId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.MaxYears).InclusiveBetween(1, 10);
    }
}