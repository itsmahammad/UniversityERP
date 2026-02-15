using FluentValidation;
using System.Net.Mail;
using UniversityERP.Infrastructure.Dtos.UserDtos;

namespace UniversityERP.Infrastructure.Validators.UserValidators;

public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
{
    public UserUpdateDtoValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("FullName is required.")
            .MaximumLength(200).WithMessage("FullName max length is 200.");

        RuleFor(x => x.PositionTitle)
            .MaximumLength(200).WithMessage("PositionTitle max length is 200.")
            .When(x => !string.IsNullOrWhiteSpace(x.PositionTitle));

        RuleFor(x => x.PersonalEmail)
            .Must(BeValidEmail).WithMessage("PersonalEmail is not a valid email address.")
            .When(x => !string.IsNullOrWhiteSpace(x.PersonalEmail));
    }

    private static bool BeValidEmail(string? email)
        => string.IsNullOrWhiteSpace(email) || MailAddress.TryCreate(email, out _);
}