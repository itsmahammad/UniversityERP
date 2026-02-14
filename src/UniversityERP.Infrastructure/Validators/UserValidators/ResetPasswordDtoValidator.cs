using FluentValidation;
using UniversityERP.Infrastructure.Dtos.UserDtos;

namespace UniversityERP.Infrastructure.Validators.UserValidators;

public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
{
    public ResetPasswordDtoValidator()
    {
        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(64);
    }
}