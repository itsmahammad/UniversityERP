using FluentValidation;
using UniversityERP.Infrastructure.Dtos.StudentSemesterEnrollmentDtos;

namespace UniversityERP.Infrastructure.Validators.StudentSemesterEnrollmentValidators;

public class StudentSemesterEnrollmentCreateDtoValidator : AbstractValidator<StudentSemesterEnrollmentCreateDto>
{
    public StudentSemesterEnrollmentCreateDtoValidator()
    {
        RuleFor(x => x.StudentId).NotEmpty();
        RuleFor(x => x.SemesterId).NotEmpty();
        RuleFor(x => x.StartingCgpa)
            .InclusiveBetween(0m, 4m)
            .When(x => x.StartingCgpa.HasValue);
        RuleFor(x => x.Notes)
            .MaximumLength(1000);
    }
}
