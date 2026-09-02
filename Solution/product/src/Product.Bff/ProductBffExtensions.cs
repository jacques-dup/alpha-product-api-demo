using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Product.Bff;

public static class ProductBffExtensions
{
    public static IServiceCollection AddProductBff(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment? environment = null)
        => ProductBffAdapter.RegisterServices(services, configuration, environment);

    public static WebApplication UseProductBff(this WebApplication app)
        => ProductBffAdapter.Use(app);

    public static IEndpointRouteBuilder MapProductBff(this IEndpointRouteBuilder endpoints)
        => ProductBffAdapter.MapEndpoints(endpoints);
}
