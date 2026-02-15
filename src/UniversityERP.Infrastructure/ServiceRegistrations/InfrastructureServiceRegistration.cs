﻿using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniversityERP.Infrastructure.Options;
using UniversityERP.Infrastructure.Services.Abstractions;
using UniversityERP.Infrastructure.Services.Implementations;
using UniversityERP.Infrastructure.Validators.FacultyValidators;
using UniversityERP.Infrastructure.Validators.UserValidators;

namespace UniversityERP.Infrastructure.ServiceRegistrations;

public static class InfrastructureServiceRegistration
{

    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<JwtOptions>(config.GetSection("Jwt"));

        services.Configure<UniversityEmailOptions>(config.GetSection("UniversityEmail"));

        services.Configure<EmailOptions>(config.GetSection("Email"));
        services.AddScoped<IEmailSender, SmtpEmailSender>();

        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddAutoMapper(_ => { }, typeof(InfrastructureServiceRegistration).Assembly);

        services.AddFluentValidationAutoValidation();

        services.AddValidatorsFromAssemblyContaining<UserCreateDtoValidator>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IFacultyService, FacultyService>();
    }
}
