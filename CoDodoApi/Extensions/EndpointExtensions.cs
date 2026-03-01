using CoDodoApi.Endpoints;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace CoDodoApi.Extensions;

/// <summary>
/// Endpoint extension functions.
/// </summary>
public static class EndpointExtensions
{
    /// <summary>
    /// Adds all endpoints in the specified assembly to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add to.</param>
    /// <param name="assembly">The <see langword="assembly"/>to use when fetching services.</param>
    /// <returns></returns>
    public static IServiceCollection AddEndpoints(
        this IServiceCollection services,
        Assembly assembly)
    {
        ServiceDescriptor[] serviceDescriptors = assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }

    /// <summary>
    /// Maps all the endpoints in the <see langword="assembly"/> to the specified <see cref="WebApplication"/>.
    /// </summary>
    /// <param name="app">The application.</param>
    /// <param name="routeGroupBuilder">A RouteGroupBuilder for building the routes into groups.</param>
    /// <returns></returns>
    public static IApplicationBuilder MapEndpoints(
        this WebApplication app,
        RouteGroupBuilder? routeGroupBuilder = null)
    {
        IEnumerable<IEndpoint> endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        IEndpointRouteBuilder builder = routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (IEndpoint endpoint in endpoints)
        {
            endpoint.MapEndpoint(builder);
        }

        return app;
    }
}