using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Product.Bff.Authentication;
using Product.Domain;
using Product.WebApi.Authentication;

namespace Product.Bff.Acceptance.Tests;

public class ProductBffApiTests
{
    private WebApplicationFactory<Program> _factory = null!;
    private HttpClient _client = null!;

    [SetUp]
    public void SetUp()
    {
        _factory = new WebApplicationFactory<Program>().WithStubbedBffCookie();
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("X-CSRF", "1");
    }

    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Test]
    public async Task Reused_get_languages_returns_ok()
    {
        var response = await _client.GetAsync("/api/languages");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task Create_update_delete_language_round_trips()
    {
        var code = $"zz{Guid.NewGuid():N}"[..8];
        var created = await _client.PostAsJsonAsync("/api/languages", new Language { Code = code, IsActive = true });
        Assert.That(created.StatusCode, Is.EqualTo(HttpStatusCode.Created));

        var updated = await _client.PutAsJsonAsync($"/api/languages/{code}", new Language { Code = code, IsActive = false });
        Assert.That(updated.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var deleted = await _client.DeleteAsync($"/api/languages/{code}");
        Assert.That(deleted.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }
}

public class ProductBffAuthorizationTests
{
    [Test]
    public async Task Anonymous_api_is_unauthorized()
    {
        using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        var response = await client.GetAsync("/api/languages");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task Bearer_token_does_not_satisfy_bff_cookie_routes()
    {
        using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "not-a-real-token");
        client.DefaultRequestHeaders.Add("X-CSRF", "1");

        var response = await client.GetAsync("/api/languages");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task Unlisted_account_is_forbidden()
    {
        using var factory = new WebApplicationFactory<Program>().WithStubbedBffCookie(sub: "not-on-the-list");
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-CSRF", "1");

        var response = await client.GetAsync("/api/languages");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    public async Task Read_only_scope_cannot_write_via_bff()
    {
        using var factory = new WebApplicationFactory<Program>().WithStubbedBffCookie(scope: ProductApiAuthentication.DefaultReadScope);
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-CSRF", "1");

        var response = await client.PostAsJsonAsync("/api/languages", new Language { Code = "no", IsActive = true });
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }
}

public sealed class ProductBffTestAuthHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string SchemeName = "ProductBffTest";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var sub = Context.RequestServices.GetRequiredService<ProductBffTestIdentity>().Sub;
        var scope = Context.RequestServices.GetRequiredService<ProductBffTestIdentity>().Scope;
        var identity = new ClaimsIdentity(
            [
                new Claim("sub", sub),
                new Claim(ProductApiScopeRequirement.ScopeClaimType, scope)
            ],
            SchemeName);
        return Task.FromResult(AuthenticateResult.Success(
            new AuthenticationTicket(new ClaimsPrincipal(identity), SchemeName)));
    }
}

public sealed class ProductBffTestIdentity
{
    public string Sub { get; init; } = "portal-staff";
    public string Scope { get; init; } = ProductBffAuthentication.DefaultWriteScope;
}

public static class ProductBffTestFactory
{
    public static WebApplicationFactory<Program> WithStubbedBffCookie(
        this WebApplicationFactory<Program> factory,
        string sub = "portal-staff",
        string scope = ProductBffAuthentication.DefaultWriteScope)
        => factory.WithWebHostBuilder(builder =>
        {
            builder.UseSetting("Portal:AllowList:0", "portal-staff");
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton(new ProductBffTestIdentity { Sub = sub, Scope = scope });
                services.AddAuthentication()
                    .AddScheme<AuthenticationSchemeOptions, ProductBffTestAuthHandler>(
                        ProductBffTestAuthHandler.SchemeName, _ => { });

                services.AddAuthorizationBuilder()
                    .AddPolicy(ProductBffAuthentication.AccessPolicy, policy => policy
                        .AddAuthenticationSchemes(ProductBffTestAuthHandler.SchemeName)
                        .RequireAuthenticatedUser()
                        .AddRequirements(new ProductBffScopeRequirement(ProductBffAuthentication.DefaultWriteScope))
                        .AddRequirements(new PortalAllowListRequirement()));
            });
        });
}
