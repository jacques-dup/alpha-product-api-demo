using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Product.WebApi.Authentication;

namespace Product.WebApi.Acceptance.Tests;

/// <summary>
/// Stands in for a client-credentials token from the IDP so route behaviour can be
/// exercised without a live token. Authentication itself is proven by the anonymous
/// and bad-token cases, which run against the real JWT bearer scheme.
/// </summary>
public sealed class ProductApiTestAuthHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string SchemeName = "ProductApiTest";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        => Task.FromResult(ProductApiTestPrincipal.Ticket(
            SchemeName,
            ProductApiAuthentication.DefaultReadScope));
}

/// <summary>A token that authenticates but carries the wrong scope: expected to be forbidden.</summary>
public sealed class ProductApiScopelessTestAuthHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string SchemeName = "ProductApiScopelessTest";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        => Task.FromResult(ProductApiTestPrincipal.Ticket(SchemeName, "alpha.idp.admin"));
}

internal static class ProductApiTestPrincipal
{
    public static AuthenticateResult Ticket(string scheme, string scope)
    {
        var identity = new ClaimsIdentity(
            [
                new Claim("client_id", "product-webapi-tests"),
                new Claim(ProductApiScopeRequirement.ScopeClaimType, scope)
            ],
            scheme);

        return AuthenticateResult.Success(
            new AuthenticationTicket(new ClaimsPrincipal(identity), scheme));
    }
}

public static class ProductApiTestFactory
{
    /// <summary>
    /// Repoints the <see cref="ProductApiAuthentication.ReadPolicy"/> at a stub scheme.
    /// <c>AuthorizationOptions.AddPolicy</c> overwrites by name, and test services are
    /// configured last, so this replaces the production policy rather than adding to it.
    /// </summary>
    public static WebApplicationFactory<Program> WithStubbedBearer(this WebApplicationFactory<Program> factory)
        => factory.WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
        {
            services.AddAuthentication()
                .AddScheme<AuthenticationSchemeOptions, ProductApiTestAuthHandler>(
                    ProductApiTestAuthHandler.SchemeName, _ => { });

            RepointReadPolicy(services, ProductApiTestAuthHandler.SchemeName);
        }));

    /// <summary>Same wiring, but the stub token carries a scope other than <c>alpha.idp.read</c>.</summary>
    public static WebApplicationFactory<Program> WithScopelessBearer(this WebApplicationFactory<Program> factory)
        => factory.WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
        {
            services.AddAuthentication()
                .AddScheme<AuthenticationSchemeOptions, ProductApiScopelessTestAuthHandler>(
                    ProductApiScopelessTestAuthHandler.SchemeName, _ => { });

            RepointReadPolicy(services, ProductApiScopelessTestAuthHandler.SchemeName);
        }));

    private static void RepointReadPolicy(IServiceCollection services, string scheme)
        => services.AddAuthorizationBuilder()
            .AddPolicy(ProductApiAuthentication.ReadPolicy, policy => policy
                .AddAuthenticationSchemes(scheme)
                .RequireAuthenticatedUser()
                .AddRequirements(new ProductApiScopeRequirement(ProductApiAuthentication.DefaultReadScope)));
}
