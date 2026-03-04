using FluentValidation;
using UniversityERP.Infrastructure.Dtos.AcademicCourseDtos;

namespace UniversityERP.Infrastructure.Validators.AcademicCourseValidators;

public class AcademicCourseCreateDtoValidator : AbstractValidator<AcademicCourseCreateDto>
{
    public AcademicCourseCreateDtoValidator()
    {
        RuleFor(x => x.OwningDepartmentId).NotEmpty();
        RuleFor(x => x.Code).NotEmpty().MaximumLength(30);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(2000);
        RuleFor(x => x.EctsCredits).InclusiveBetween(1, 60);
    }
}