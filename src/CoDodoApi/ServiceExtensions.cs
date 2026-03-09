using Application.Abstractions.Data;
using CoDodoApi.Database;
using CoDodoApi.OpenApi;
using CoDodoApi.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CoDodoApi;

public static class ServiceExtensions
{
    public static IServiceCollection AddConfiguredCors(
        this IServiceCollection services)
    {
        services.AddCors(o => o
            .AddDefaultPolicy(p => p
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()));

        return services;
    }

    public static IServiceCollection AddConfiguredAuthentication(
        this IServiceCollection services)
    {
        services
            .AddAuthentication("BasicAuthentication")
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>
                ("BasicAuthentication", options =>
                {
                });

        return services;
    }

    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("Database");
        Ensure.NotNullOrEmpty(connectionString);

        services.AddDbContext<ApplicationDbContext>(
            (sp, options) => options
                .UseNpgsql(connectionString)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
                .UseSnakeCaseNamingConvention());

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

        return services;
    }
}