﻿using System.Net.Mail;
using FluentValidation;
using UniversityERP.Infrastructure.Dtos.UserDtos;

namespace UniversityERP.Infrastructure.Validators.UserValidators;

public class UserCreateDtoValidator : AbstractValidator<UserCreateDto>
{
    public UserCreateDtoValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.FinCode)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(16)
            .Matches("^[A-Za-z0-9]+$")
            .WithMessage("FinCode must be alphanumeric and between 5 and 16 characters.");

        RuleFor(x => x.PersonalEmail)
            .Must(BeValidEmail)
            .When(x => !string.IsNullOrWhiteSpace(x.PersonalEmail))
            .WithMessage("PersonalEmail is not a valid email address.");

        RuleFor(x => x.PositionTitle).MaximumLength(100);

        RuleFor(x => x.Role).IsInEnum();
    }

    private static bool BeValidEmail(string? email)
        => string.IsNullOrWhiteSpace(email) || MailAddress.TryCreate(email, out _);
}