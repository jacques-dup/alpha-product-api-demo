using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Product.Domain;

namespace Product.ProductStore;

/// <summary>
/// Driven adapter for Postgres. ApplicationRoot plugs this so Bff and WebApi
/// resolve <see cref="IProductRepository"/> in-process. DbContext, options, and
/// the repository are scoped (not singleton) so each request gets its own unit of work.
/// </summary>
public sealed class ProductStoreAdapter
{
    public static IServiceCollection RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var connectionString = configuration.GetConnectionString("ProductStore");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Connection string 'ProductStore' is required.");
        }

        services.AddDbContext<ProductDbContext>(
            options => options.UseNpgsql(connectionString),
            contextLifetime: ServiceLifetime.Scoped,
            optionsLifetime: ServiceLifetime.Scoped);
        services.AddScoped<IProductRepository, ProductRepository>();
        return services;
    }
}
