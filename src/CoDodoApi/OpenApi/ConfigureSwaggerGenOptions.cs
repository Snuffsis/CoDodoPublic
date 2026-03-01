using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
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
        options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("BasicAuthentication", doc)] = []
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