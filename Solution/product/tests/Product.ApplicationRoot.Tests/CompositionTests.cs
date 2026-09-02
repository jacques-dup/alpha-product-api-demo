using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Product.ApplicationRoot;
using Product.Domain;
using Product.ProductStore;

namespace Product.ApplicationRoot.Tests;

public class CompositionTests
{
    [Test]
    public void Program_type_is_public()
    {
        Assert.That(typeof(Program).IsPublic, Is.True);
    }

    [Test]
    public void AddProductModules_registers_repository_and_dbcontext_as_scoped()
    {
        var services = new ServiceCollection();
        services.AddProductModules(InMemoryStoreConfiguration());

        AssertAllScoped<ProductDbContext>(services);
        AssertAllScoped<DbContextOptions<ProductDbContext>>(services);
        AssertAllScoped<IProductRepository>(services);

        using var provider = services.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateScopes = true
        });

        Assert.Throws<InvalidOperationException>(() => provider.GetRequiredService<IProductRepository>());

        using var scope = provider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
        var db = scope.ServiceProvider.GetRequiredService<ProductDbContext>();

        Assert.That(repository, Is.InstanceOf<ProductRepository>());
        Assert.That(db, Is.Not.Null);
        Assert.That(
            scope.ServiceProvider.GetRequiredService<IProductRepository>(),
            Is.SameAs(repository));
    }

    private static IConfiguration InMemoryStoreConfiguration()
        => new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:ProductStore"] = "Host=localhost;Database=product_service;Username=jdples",
                ["Identity:Authority"] = "https://dev.auth.alpha.org"
            })
            .Build();

    private static void AssertAllScoped<TService>(IServiceCollection services)
        where TService : class
    {
        var descriptors = services.Where(descriptor => descriptor.ServiceType == typeof(TService)).ToArray();
        Assert.That(descriptors, Is.Not.Empty, $"Expected a registration for {typeof(TService).Name}.");
        Assert.That(
            descriptors.Select(descriptor => descriptor.Lifetime),
            Is.All.EqualTo(ServiceLifetime.Scoped));
    }
}
