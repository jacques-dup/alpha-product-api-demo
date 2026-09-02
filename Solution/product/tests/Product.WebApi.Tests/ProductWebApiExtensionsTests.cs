using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Product.Domain;
using Product.WebApi.Endpoints;

namespace Product.WebApi.Tests;

public class ProductWebApiExtensionsTests
{
    [Test]
    public void WebApi_adapter_extensions_are_defined()
    {
        Assert.That(typeof(ProductWebApiExtensions).IsAbstract, Is.True);
    }

    [Test]
    public void Adapter_registers_services_and_maps_the_product_group()
    {
        var builder = WebApplication.CreateBuilder();
        ProductWebApiAdapter.RegisterServices(builder.Services, IdentityConfiguration());
        var app = builder.Build();
        Assert.That(() => ProductWebApiAdapter.Use(app), Throws.Nothing);
        Assert.That(() => ProductWebApiAdapter.MapEndpoints(app), Throws.Nothing);
    }

    [Test]
    public void Adapter_refuses_to_start_without_an_identity_authority()
    {
        var configuration = new ConfigurationBuilder().AddInMemoryCollection().Build();
        var builder = WebApplication.CreateBuilder();

        Assert.That(
            () => ProductWebApiAdapter.RegisterServices(builder.Services, configuration),
            Throws.InstanceOf<InvalidOperationException>());
    }

    private static IConfiguration IdentityConfiguration()
        => new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Identity:Authority"] = "https://dev.auth.alpha.org"
            })
            .Build();
}

public class ProductPayloadMapperTests
{
    [Test]
    public void List_filters_by_family_code_as_course_type()
    {
        var family = new ProductFamily { Id = Guid.NewGuid(), Code = "alpha-film-series", Name = "AFS" };
        var included = new Domain.Product
        {
            Id = Guid.NewGuid(),
            FamilyId = family.Id,
            Code = "alpha-film-series-africa",
            Title = "AFS Africa",
            ContentLanguage = "en"
        };
        var otherFamily = new ProductFamily { Id = Guid.NewGuid(), Code = "marriage", Name = "Marriage" };
        var excluded = new Domain.Product
        {
            Id = Guid.NewGuid(),
            FamilyId = otherFamily.Id,
            Code = "marriage-course",
            Title = "Marriage Course",
            ContentLanguage = "en"
        };

        var snapshot = new ProductSnapshot
        {
            Products = [included, excluded],
            Families = [family, otherFamily],
            Tags = [],
            ProductTags = [],
            ProductMarkets = [],
            Items = [],
            Assets = [],
            AssetMarkets = []
        };

        var result = ProductPayloadMapper.ListPayloads(snapshot, "alpha-film-series", null, null, null);

        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Code, Is.EqualTo("alpha-film-series-africa"));
    }
}
