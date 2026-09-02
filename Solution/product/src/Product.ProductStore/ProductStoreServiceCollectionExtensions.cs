using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Product.ProductStore;

public static class ProductStoreServiceCollectionExtensions
{
    public static IServiceCollection AddProductStore(this IServiceCollection services, IConfiguration configuration)
        => ProductStoreAdapter.RegisterServices(services, configuration);
}
