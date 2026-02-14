using FluentValidation;
using UniversityERP.Infrastructure.Dtos.UserDtos;

namespace UniversityERP.Infrastructure.Validators.UserValidators;

public class UserCreateDtoValidator : AbstractValidator<UserCreateDto>
{
    public UserCreateDtoValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(200);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6).MaximumLength(64);
        RuleFor(x => x.PositionTitle).MaximumLength(100);
        RuleFor(x => x.Role).IsInEnum();
    }
}