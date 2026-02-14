using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using UniversityERP.API.Middlewares;
using UniversityERP.Application.ServiceRegistrations;
using UniversityERP.Infrastructure.Options;
using UniversityERP.Infrastructure.ServiceRegistrations;
namespace UniversityERP.API;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        builder.Services.AddApplicationServices(builder.Configuration);
        builder.Services.AddInfrastructureServices(builder.Configuration);

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "UniversityERP API", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>()
                }
            });
        });


        builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var jwt = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()!;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,

                    ValidIssuer = jwt.Issuer,
                    ValidAudience = jwt.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
                    ClockSkew = TimeSpan.FromSeconds(30)
                };
            });

        builder.Services.AddAuthorization();

        var app = builder.Build();

        //pipeline
        app.UseMiddleware<GlobalExceptionHandler>();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        //Seed admin user (DEV ONLY)
        await SeedAdminAsync(app);

        await app.RunAsync();
    }

    private static async Task SeedAdminAsync(WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
            return;

        using var scope = app.Services.CreateScope();

        var userRepo = scope.ServiceProvider.GetRequiredService<UniversityERP.Application.Repositories.Abstractions.IUserRepository>();

        var adminEmail = "admin@uni.local";

        var exists = await userRepo.ExistsByEmailAsync(adminEmail, ignoreQueryFilter: true);
        if (exists) return;

        var admin = new UniversityERP.Domain.Entities.User
        {
            FullName = "System Admin",
            Email = adminEmail,
            Role = UniversityERP.Domain.Enums.UserRole.Admin,
            IsActive = true
        };

        var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<UniversityERP.Domain.Entities.User>();
        admin.PasswordHash = hasher.HashPassword(admin, "Admin123!");

        await userRepo.AddAsync(admin);
        await userRepo.SaveChangesAsync();
    }
}
