using FluentValidation;
using UniversityERP.Infrastructure.Dtos.TeacherDtos;

namespace UniversityERP.Infrastructure.Validators.TeacherValidators;

public class TeacherUpdateDtoValidator : AbstractValidator<TeacherUpdateDto>
{
    public TeacherUpdateDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.DepartmentId).NotEmpty();
        RuleFor(x => x.HireDate).NotEmpty();
    }
}