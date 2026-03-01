namespace CoDodoApi.Endpoints;

/// <summary>
/// Interface for endpoints.
/// </summary>
public interface IEndpoint
{
    /// <summary>
    /// Maps the endpoint.
    /// </summary>
    /// <param name="app"></param>
    void MapEndpoint(IEndpointRouteBuilder app);
}