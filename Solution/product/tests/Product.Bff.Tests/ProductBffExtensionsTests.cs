using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Product.Bff.Authentication;

namespace Product.Bff.Tests;

public class ProductBffExtensionsTests
{
    [Test]
    public void Bff_adapter_extensions_are_defined()
    {
        Assert.That(typeof(ProductBffExtensions).IsAbstract, Is.True);
    }
}

public class PortalAllowListHandlerTests
{
    [Test]
    public void Empty_list_denies()
    {
        var user = Principal("sub-1", "a@b.c");
        Assert.That(PortalAllowListHandler.IsAllowed(user, []), Is.False);
    }

    [Test]
    public void Matching_sub_is_allowed()
    {
        var user = Principal("sub-1", "a@b.c");
        Assert.That(PortalAllowListHandler.IsAllowed(user, ["sub-1"]), Is.True);
    }

    [Test]
    public void Matching_email_is_allowed()
    {
        var user = Principal("sub-1", "staff@alpha.org");
        Assert.That(PortalAllowListHandler.IsAllowed(user, ["staff@alpha.org"]), Is.True);
    }

    [Test]
    public void Unknown_account_is_denied()
    {
        var user = Principal("sub-1", "a@b.c");
        Assert.That(PortalAllowListHandler.IsAllowed(user, ["someone-else"]), Is.False);
    }

    private static ClaimsPrincipal Principal(string sub, string email)
    {
        var identity = new ClaimsIdentity(
            [new Claim("sub", sub), new Claim("email", email)],
            ProductBffAuthentication.CookieScheme);
        return new ClaimsPrincipal(identity);
    }
}

public class ProductBffAdapterTests
{
    [Test]
    public void Adapter_registers_and_maps_without_oidc_client()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Identity:Authority"] = "https://dev.auth.alpha.org"
            })
            .Build();

        var builder = WebApplication.CreateBuilder();
        ProductBffAdapter.RegisterServices(builder.Services, configuration);
        var app = builder.Build();
        Assert.That(() => ProductBffAdapter.Use(app), Throws.Nothing);
        Assert.That(() => ProductBffAdapter.MapEndpoints(app), Throws.Nothing);
    }
}

public class ProductBffOidcTests
{
    [Test]
    public void Client_id_enables_oidc_challenge_to_the_idp()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Identity:Authority"] = "https://dev.auth.alpha.org",
                ["Bff:ClientId"] = "product_portal_bff",
                ["Bff:ClientSecret"] = "test-secret"
            })
            .Build();

        var builder = WebApplication.CreateBuilder();
        builder.Environment.EnvironmentName = Environments.Development;
        ProductBffAdapter.RegisterServices(builder.Services, configuration, builder.Environment);
        using var app = builder.Build();

        var auth = app.Services.GetRequiredService<IOptions<AuthenticationOptions>>().Value;
        Assert.That(auth.DefaultChallengeScheme, Is.EqualTo(ProductBffAuthentication.OidcScheme));
        Assert.That(auth.DefaultSignOutScheme, Is.EqualTo(ProductBffAuthentication.OidcScheme));
        Assert.That(auth.DefaultSignInScheme, Is.EqualTo(ProductBffAuthentication.CookieScheme));
        Assert.That(auth.DefaultScheme, Is.EqualTo(ProductBffAuthentication.CookieScheme));

        var oidc = app.Services.GetRequiredService<IOptionsMonitor<OpenIdConnectOptions>>()
            .Get(ProductBffAuthentication.OidcScheme);
        Assert.That(oidc.Authority, Is.EqualTo("https://dev.auth.alpha.org"));
        Assert.That(oidc.CallbackPath.Value, Is.EqualTo("/signin-oidc"));
        Assert.That(oidc.SignedOutCallbackPath.Value, Is.EqualTo("/signout-callback-oidc"));
        Assert.That(oidc.ResponseType, Is.EqualTo("code"));
        Assert.That(oidc.Scope, Does.Contain("openid"));
        Assert.That(oidc.Scope, Does.Contain("alpha.idp.readwrite"));
        Assert.That(oidc.SignInScheme, Is.EqualTo(ProductBffAuthentication.CookieScheme));
        Assert.That(oidc.CorrelationCookie.SameSite, Is.EqualTo(SameSiteMode.Lax));
        Assert.That(oidc.NonceCookie.SameSite, Is.EqualTo(SameSiteMode.Lax));
    }
}

public class ProductBffCountryCodeTests
{
    [Test]
    public void Configured_value_wins()
    {
        Assert.That(
            ProductBffAuthentication.ResolveCountryCode(
                Config(("Bff:CountryCode", "ke")),
                Env(Environments.Production)),
            Is.EqualTo("ke"));
    }

    [Test]
    public void Missing_config_in_production_is_global()
    {
        Assert.That(
            ProductBffAuthentication.ResolveCountryCode(Config(), Env(Environments.Production)),
            Is.EqualTo(ProductBffAuthentication.ProductionCountryCode));
    }

    [Test]
    public void Missing_config_outside_production_is_za()
    {
        Assert.That(
            ProductBffAuthentication.ResolveCountryCode(Config(), Env(Environments.Development)),
            Is.EqualTo(ProductBffAuthentication.DefaultCountryCode));
        Assert.That(
            ProductBffAuthentication.ResolveCountryCode(Config(), environment: null),
            Is.EqualTo(ProductBffAuthentication.DefaultCountryCode));
    }

    private static IConfiguration Config(params (string Key, string Value)[] pairs)
        => new ConfigurationBuilder()
            .AddInMemoryCollection(pairs.ToDictionary(pair => pair.Key, pair => (string?)pair.Value))
            .Build();

    private static IHostEnvironment Env(string name)
    {
        var builder = WebApplication.CreateBuilder();
        builder.Environment.EnvironmentName = name;
        return builder.Environment;
    }
}
