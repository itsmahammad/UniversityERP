using FluentValidation;
using UniversityERP.Infrastructure.Dtos.UserDtos;

namespace UniversityERP.Infrastructure.Validators.UserValidators;

public class UserCreateDtoValidator : AbstractValidator<UserCreateDto>
{
    public UserCreateDtoValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(200);
        RuleFor(x => x.PersonalEmail).NotEmpty().EmailAddress().MaximumLength(200);
        RuleFor(x => x.PositionTitle).MaximumLength(100);
        RuleFor(x => x.Role).IsInEnum();
    }
}