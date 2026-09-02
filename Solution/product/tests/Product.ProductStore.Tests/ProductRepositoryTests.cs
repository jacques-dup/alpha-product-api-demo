using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Product.Domain;

namespace Product.ProductStore.Tests;

public class ProductRepositoryTests
{
    [Test]
    public void ProductRepository_implements_IProductRepository()
    {
        Assert.That(typeof(IProductRepository).IsAssignableFrom(typeof(ProductRepository)), Is.True);
    }

    [Test]
    public void DbContext_maps_all_catalog_tables()
    {
        var options = new DbContextOptionsBuilder<ProductDbContext>()
            .UseNpgsql("Host=localhost;Database=product_service;Username=jdples")
            .Options;

        using var db = new ProductDbContext(options);
        var tables = db.Model.GetEntityTypes()
            .Select(entity => entity.GetTableName())
            .OrderBy(name => name)
            .ToArray();

        Assert.That(
            tables,
            Is.EqualTo(new[]
            {
                "asset",
                "asset_market",
                "language",
                "market",
                "product",
                "product_family",
                "product_item",
                "product_market",
                "product_tag",
                "tag"
            }));
    }

    [Test]
    public void AddProductStore_registers_dbcontext_and_repository_as_scoped()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:ProductStore"] = "Host=localhost;Database=product_service;Username=jdples"
            })
            .Build();

        var services = new ServiceCollection();
        services.AddProductStore(configuration);

        Assert.That(
            services.Single(descriptor => descriptor.ServiceType == typeof(ProductDbContext)).Lifetime,
            Is.EqualTo(ServiceLifetime.Scoped));
        Assert.That(
            services.Where(descriptor => descriptor.ServiceType == typeof(DbContextOptions<ProductDbContext>))
                .Select(descriptor => descriptor.Lifetime),
            Is.All.EqualTo(ServiceLifetime.Scoped));
        Assert.That(
            services.Single(descriptor => descriptor.ServiceType == typeof(IProductRepository)).Lifetime,
            Is.EqualTo(ServiceLifetime.Scoped));

        using var provider = services.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateScopes = true
        });
        using var scope = provider.CreateScope();

        Assert.That(scope.ServiceProvider.GetService<ProductDbContext>(), Is.Not.Null);
        Assert.That(scope.ServiceProvider.GetService<IProductRepository>(), Is.InstanceOf<ProductRepository>());
    }
}
