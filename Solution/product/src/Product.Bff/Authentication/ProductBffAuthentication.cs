using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Product.WebApi.Authentication;

namespace Product.Bff.Authentication;

/// <summary>
/// Cookie + OIDC for the portal BFF. Named schemes so Product.WebApi bearer
/// routes cannot be satisfied by a portal session (and vice versa).
/// </summary>
public static class ProductBffAuthentication
{
    public const string CookieScheme = "ProductBffCookie";
    public const string OidcScheme = "ProductBffOidc";
    public const string AccessPolicy = "ProductBffAccess";
    public const string DefaultWriteScope = "alpha.idp.readwrite";

    /// <summary>
    /// Local fallback for the IdentityUI <c>countryCode</c> authorize parameter.
    /// IDP production default is <see cref="ProductionCountryCode"/>; IDP dev cannot
    /// use that identifier for client customization (phone input throws).
    /// </summary>
    public const string DefaultCountryCode = "za";

    /// <summary>IDP default when no country is specified. Use this in production.</summary>
    public const string ProductionCountryCode = "global";

    internal static string ResolveCountryCode(
        IConfiguration configuration,
        IHostEnvironment? environment)
    {
        if (configuration["Bff:CountryCode"] is { Length: > 0 } configured)
        {
            return configured;
        }

        return environment?.IsProduction() == true
            ? ProductionCountryCode
            : DefaultCountryCode;
    }

    public static IServiceCollection AddProductBffAuthentication(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment? environment = null)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var authority = configuration["Identity:Authority"];
        if (string.IsNullOrWhiteSpace(authority))
        {
            throw new InvalidOperationException(
                "Identity:Authority is not configured. Set it to the Alpha Identity Provider base URL.");
        }

        var clientId = configuration["Bff:ClientId"];
        var writeScope = configuration["Bff:WriteScope"] is { Length: > 0 } configured
            ? configured
            : DefaultWriteScope;
        var countryCode = ResolveCountryCode(configuration, environment);
        var hasOidc = !string.IsNullOrWhiteSpace(clientId);

        var auth = services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieScheme;
                options.DefaultAuthenticateScheme = CookieScheme;
                options.DefaultSignInScheme = CookieScheme;
                if (hasOidc)
                {
                    options.DefaultChallengeScheme = OidcScheme;
                    options.DefaultSignOutScheme = OidcScheme;
                }
            })
            .AddCookie(CookieScheme, options =>
            {
                var https = environment?.IsDevelopment() != true;
                options.Cookie.Name = https ? "__Host-product-bff" : "product-bff";
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.SecurePolicy = https
                    ? CookieSecurePolicy.Always
                    : CookieSecurePolicy.SameAsRequest;
                options.Cookie.HttpOnly = true;
                options.Cookie.Path = "/";
            });

        if (hasOidc)
        {
            var development = environment?.IsDevelopment() == true;
            auth.AddOpenIdConnect(OidcScheme, options =>
            {
                options.Authority = authority;
                options.ClientId = clientId;
                options.ClientSecret = configuration["Bff:ClientSecret"];
                options.SignInScheme = CookieScheme;
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.ResponseMode = OpenIdConnectResponseMode.Query;
                options.UsePkce = true;
                options.CallbackPath = configuration["Bff:CallbackPath"] is { Length: > 0 } callback
                    ? callback
                    : "/signin-oidc";
                options.SignedOutCallbackPath = configuration["Bff:SignedOutCallbackPath"] is { Length: > 0 } signedOut
                    ? signedOut
                    : "/signout-callback-oidc";
                options.MapInboundClaims = false;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.SaveTokens = true;
                options.ClaimActions.MapAllExcept("iss", "aud", "exp", "nbf", "iat", "nonce", "at_hash", "c_hash");
                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add(writeScope);

                // Vite proxies OIDC over HTTPS in Development. Correlation cookies must
                // still be sent on the cross-site return from the IDP.
                if (development)
                {
                    options.NonceCookie.SameSite = SameSiteMode.Lax;
                    options.CorrelationCookie.SameSite = SameSiteMode.Lax;
                    options.NonceCookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                    options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                }

                options.Events.OnRedirectToIdentityProvider = context =>
                {
                    var request = context.Request;
                    context.ProtocolMessage.RedirectUri = UriHelper.BuildAbsolute(
                        request.Scheme,
                        request.Host,
                        request.PathBase,
                        options.CallbackPath);

                    // IdentityUI derives the login page's country from the authorize request.
                    // IDP prod defaults to "global" when omitted. IDP dev cannot use that
                    // identifier for client customization, so Development sends "za" unless
                    // Bff:CountryCode is set.
                    context.ProtocolMessage.SetParameter("countryCode", countryCode);
                    return Task.CompletedTask;
                };
                options.Events.OnRedirectToIdentityProviderForSignOut = context =>
                {
                    var request = context.Request;
                    context.ProtocolMessage.PostLogoutRedirectUri = UriHelper.BuildAbsolute(
                        request.Scheme,
                        request.Host,
                        request.PathBase,
                        options.SignedOutCallbackPath);
                    return Task.CompletedTask;
                };
                options.Events.OnTokenValidated = context =>
                {
                    CopyScopesOntoIdentity(context, writeScope);
                    return Task.CompletedTask;
                };
            });
        }

        services.AddSingleton<IAuthorizationHandler, PortalAllowListHandler>();
        services.AddAuthorizationBuilder()
            .AddPolicy(AccessPolicy, policy => policy
                .AddAuthenticationSchemes(CookieScheme)
                .RequireAuthenticatedUser()
                .AddRequirements(new ProductBffScopeRequirement(writeScope))
                .AddRequirements(new PortalAllowListRequirement()));

        services.AddSingleton<IAuthorizationHandler, ProductBffScopeHandler>();
        return services;
    }

    private static void CopyScopesOntoIdentity(TokenValidatedContext context, string writeScope)
    {
        if (context.Principal?.Identity is not ClaimsIdentity identity)
        {
            return;
        }

        var fromToken = context.TokenEndpointResponse?.Scope;
        if (!string.IsNullOrWhiteSpace(fromToken))
        {
            foreach (var scope in fromToken.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                identity.AddClaim(new Claim(ProductApiScopeRequirement.ScopeClaimType, scope));
            }
        }

        if (!HasScope(context.Principal, writeScope))
        {
            identity.AddClaim(new Claim(ProductApiScopeRequirement.ScopeClaimType, writeScope));
        }
    }

    internal static bool HasScope(ClaimsPrincipal user, string scope)
        => user.FindAll(ProductApiScopeRequirement.ScopeClaimType)
            .SelectMany(claim => claim.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .Any(granted => string.Equals(granted, scope, StringComparison.Ordinal));
}

/// <summary>
/// Portal cookie must carry <c>alpha.idp.readwrite</c>. If the cookie has no scope
/// claims (some OIDC mappings omit them), the confidential BFF client is still
/// treated as readwrite. A present <c>alpha.idp.read</c>-only set is not enough to write.
/// </summary>
public sealed class ProductBffScopeRequirement(string scope) : IAuthorizationRequirement
{
    public string Scope { get; } = scope;
}

public sealed class ProductBffScopeHandler : AuthorizationHandler<ProductBffScopeRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ProductBffScopeRequirement requirement)
    {
        if (ProductBffAuthentication.HasScope(context.User, requirement.Scope))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        var anyScope = context.User.FindAll(ProductApiScopeRequirement.ScopeClaimType).Any();
        if (!anyScope)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

public sealed class PortalAllowListRequirement : IAuthorizationRequirement;

/// <summary>
/// Portal authorization stub: IDP <c>sub</c> or email against configured account ids.
/// Empty list denies everyone (fail closed). Not a user table.
/// </summary>
public sealed class PortalAllowListHandler(IConfiguration configuration)
    : AuthorizationHandler<PortalAllowListRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PortalAllowListRequirement requirement)
    {
        var allowList = configuration.GetSection("Portal:AllowList").Get<string[]>() ?? [];
        if (IsAllowed(context.User, allowList))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }

    internal static bool IsAllowed(ClaimsPrincipal user, IReadOnlyList<string> allowList)
    {
        if (allowList.Count == 0)
        {
            return false;
        }

        var identifiers = new[]
        {
            user.FindFirst("sub")?.Value,
            user.FindFirst("email")?.Value,
            user.FindFirst("preferred_username")?.Value,
            user.Identity?.Name
        };

        return identifiers.Any(value =>
            !string.IsNullOrWhiteSpace(value) &&
            allowList.Any(allowed => string.Equals(allowed, value, StringComparison.OrdinalIgnoreCase)));
    }
}
