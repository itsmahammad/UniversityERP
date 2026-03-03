using FluentValidation;
using UniversityERP.Infrastructure.Dtos.SemesterDtos;

namespace UniversityERP.Infrastructure.Validators.SemesterValidators;

public class SemesterCreateDtoValidator : AbstractValidator<SemesterCreateDto>
{
    public SemesterCreateDtoValidator()
    {
        RuleFor(x => x.AcademicYearId).NotEmpty();
        RuleFor(x => x.MaxCredits).InclusiveBetween(1, 60);

        RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate);
        RuleFor(x => x.RegistrationEnd).GreaterThanOrEqualTo(x => x.RegistrationStart);
        RuleFor(x => x.ExamEnd).GreaterThanOrEqualTo(x => x.ExamStart);
    }
}