using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using UniversityERP.Infrastructure.Services.Abstractions;
using UniversityERP.Infrastructure.Services.Implementations;
using UniversityERP.Infrastructure.Validators.FacultyValidators;

namespace UniversityERP.Infrastructure.ServiceRegistrations;

public static class InfrastructureServiceRegistration
{

    public static void AddInfrastructureServices(this IServiceCollection services)
    {

        services.AddAutoMapper(_ => { }, typeof(InfrastructureServiceRegistration).Assembly);

        services.AddFluentValidationAutoValidation();


        services.AddValidatorsFromAssemblyContaining<FacultyCreateDtoValidator>();


        services.AddScoped<IFacultyService, FacultyService>();
    }
}
