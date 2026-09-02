using Product.Bff;
using Product.ProductStore;
using Product.WebApi;

namespace Product.ApplicationRoot;

/// <summary>
/// Composition root: persistence first so Bff and WebApi can inject
/// <see cref="Product.Domain.IProductRepository"/>. Bff registers last so the
/// cookie scheme is the default authenticate scheme without overriding WebApi's
/// named bearer policy.
/// </summary>
public static class ProductApplicationComposition
{
    public static IServiceCollection AddProductModules(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment? environment = null)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddProductStore(configuration);
        services.AddProductWebApi(configuration);
        services.AddProductBff(configuration, environment);
        return services;
    }
}
