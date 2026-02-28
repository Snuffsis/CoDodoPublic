using CoDodoApi.Database;
using CoDodoApi.OpenApi;
using CoDodoApi.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CoDodoApi;

public static class ServiceExtensions
{
    public static WebApplicationBuilder AddConfiguredSerilog(
        this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();

        builder.Host.UseSerilog((context, loggerConfig) =>
        {
            loggerConfig.ReadFrom.Configuration(context.Configuration);
            loggerConfig.Enrich.FromLogContext();
            loggerConfig.Enrich.WithProperty("Application", "CoDodoApi");
        });

        return builder;
    }

    public static IServiceCollection AddSwagger(
        this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.ConfigureOptions<ConfigureSwaggerGenOptions>();

        // Add JsonOptions
        services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        services.ConfigureHttpJsonOptions(o =>
        {
            o.SerializerOptions.Converters.Add(
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: true));
        });

        return services;
    }

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


        return services;
    }
}