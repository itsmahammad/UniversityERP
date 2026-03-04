using FluentValidation;
using UniversityERP.Infrastructure.Dtos.TeacherDtos;

namespace UniversityERP.Infrastructure.Validators.TeacherValidators;

public class TeacherCreateDtoValidator : AbstractValidator<TeacherCreateDto>
{
    public TeacherCreateDtoValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.DepartmentId).NotEmpty();
        RuleFor(x => x.HireDate).NotEmpty();
    }
}