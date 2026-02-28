using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CoDodoApi.OpenApi;

/// <summary>
/// Configures the swagger generation options.
/// </summary>
public class ConfigureSwaggerGenOptions : IConfigureNamedOptions<SwaggerGenOptions>
{
    /// <summary>
    /// Configures the swagger generation options.
    /// </summary>
    /// <param name="options">The options for the swagger document.</param>
    public void Configure(SwaggerGenOptions options)
    {
        options.AddSecurityDefinition("BasicAuthentication", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "basic",
            Description = "Basic Authentication",
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "BasicAuthentication",
                        },
                    Scheme = "basic",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                },
                new List<string>()
            }
        });
    }

    /// <summary>
    /// Configures the swagger generation options.
    /// </summary>
    /// <param name="options">The options for the swagger document.</param>
    /// <param name="name">The name of the swagger document.</param>
    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }
}