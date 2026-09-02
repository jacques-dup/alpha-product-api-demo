using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Product.WebApi.Authentication;
using Product.WebApi.Endpoints;
using Product.WebApi.OpenApi;

namespace Product.WebApi;

/// <summary>
/// Inbound adapter for the read Product API. ApplicationRoot plugs this on the same host;
/// it does not start its own web process.
/// </summary>
public sealed class ProductWebApiAdapter
{
    public const string RoutePrefix = "/product";

    public static IServiceCollection RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddProductApiAuthentication(configuration);
        services.AddProblemDetails();
        services.AddEndpointsApiExplorer();
        services.AddProductOpenApi(configuration);
        return services;
    }

    /// <summary>Swagger UI at <c>/swagger</c>. Mapped outside <c>/product</c> so it stays anonymous.</summary>
    public static WebApplication Use(WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);
        return ProductOpenApi.UseProductOpenApi(app);
    }

    public static IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        endpoints.MapGroup(RoutePrefix)
            .MapApi()
            .WithTags("Product")
            .WithGroupName(ProductOpenApi.DocumentName);

        return endpoints;
    }
}
