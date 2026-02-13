using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniversityERP.Application.Contexts;
using UniversityERP.Application.Interceptors;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Application.Repositories.Implementations;

namespace UniversityERP.Application.ServiceRegistrations;

public static class ApplicationServiceRegistration
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor(); // needed for BaseAuditableInterceptor

        services.AddScoped<BaseAuditableInterceptor>();

        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            var interceptor = sp.GetRequiredService<BaseAuditableInterceptor>();

            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            options.AddInterceptors(interceptor);
        });

        // services.AddScoped<IFacultyRepository, FacultyRepository>();
        // services.AddScoped<IStudentRepository, StudentRepository>();
    }
}
