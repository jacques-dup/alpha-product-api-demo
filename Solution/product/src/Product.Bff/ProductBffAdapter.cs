using Duende.Bff;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Product.Bff.Authentication;
using Product.Bff.Endpoints;
using Product.WebApi.OpenApi;

namespace Product.Bff;

/// <summary>
/// Inbound adapter for the portal BFF. ApplicationRoot plugs this on the same host;
/// it does not start its own web process. Local APIs under <c>/api</c>; no YARP.
/// </summary>
public sealed class ProductBffAdapter
{
    public const string RoutePrefix = "/api";

    public static IServiceCollection RegisterServices(
        IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment? environment = null)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var licenseKey = configuration["Bff:LicenseKey"];
        services.AddBff(options =>
        {
            if (!string.IsNullOrWhiteSpace(licenseKey))
            {
                options.LicenseKey = licenseKey;
            }
        });

        services.AddProductBffAuthentication(configuration, environment);
        return services;
    }

    public static WebApplication Use(WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);
        app.UseBff();
        return app;
    }

    public static IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        endpoints.MapBffManagementEndpoints();
        endpoints.MapGroup(RoutePrefix)
            .MapApi()
            .WithTags("Portal")
            .WithGroupName(ProductOpenApi.BffDocumentName);

        return endpoints;
    }
}
