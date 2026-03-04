using FluentValidation;
using UniversityERP.Infrastructure.Dtos.StudentDtos;

namespace UniversityERP.Infrastructure.Validators.StudentValidators;

public class StudentCreateDtoValidator : AbstractValidator<StudentCreateDto>
{
    public StudentCreateDtoValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.AcademicProgramId).NotEmpty();
        RuleFor(x => x.EnrollmentYear).InclusiveBetween(2000, 2100);
    }
}