using FluentValidation;
using UniversityERP.Infrastructure.Dtos.StudentDtos;

namespace UniversityERP.Infrastructure.Validators.StudentValidators;

public class StudentUpdateDtoValidator : AbstractValidator<StudentUpdateDto>
{
    public StudentUpdateDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.AcademicProgramId).NotEmpty();
        RuleFor(x => x.EnrollmentYear).InclusiveBetween(2000, 2100);
    }
}