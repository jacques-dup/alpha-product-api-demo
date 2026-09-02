using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Product.WebApi;

public static class ProductWebApiExtensions
{
    public static IServiceCollection AddProductWebApi(this IServiceCollection services, IConfiguration configuration)
        => ProductWebApiAdapter.RegisterServices(services, configuration);

    public static WebApplication UseProductWebApi(this WebApplication app)
        => ProductWebApiAdapter.Use(app);

    public static IEndpointRouteBuilder MapProductWebApi(this IEndpointRouteBuilder endpoints)
        => ProductWebApiAdapter.MapEndpoints(endpoints);
}
