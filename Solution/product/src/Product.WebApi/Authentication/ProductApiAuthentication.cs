using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Product.WebApi.Authentication;

/// <summary>
/// Bearer authentication for the read Product API.
/// </summary>
/// <remarks>
/// Callers present a client-credentials access token from the Alpha Identity Provider
/// carrying scope <c>alpha.idp.read</c>. There is no end-user identity on the read API.
/// <para>
/// The scheme is named rather than default, and the policy pins that scheme, so a
/// portal cookie session can never satisfy a Product.WebApi route (and vice versa)
/// once Product.Bff adds its own OIDC scheme on the same host.
/// </para>
/// </remarks>
public static class ProductApiAuthentication
{
    /// <summary>Authentication scheme owned by this adapter. Product.Bff must not reuse it.</summary>
    public const string Scheme = "ProductApiBearer";

    /// <summary>Authorization policy applied to every route in the <c>/product</c> group.</summary>
    public const string ReadPolicy = "ProductApiRead";

    /// <summary>Existing IDP scope. Do not add scopes in the IDP for this product.</summary>
    public const string DefaultReadScope = "alpha.idp.read";

    public static IServiceCollection AddProductApiAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var identity = configuration.GetSection("Identity");

        var authority = identity["Authority"];
        if (string.IsNullOrWhiteSpace(authority))
        {
            throw new InvalidOperationException(
                "Identity:Authority is not configured. Set it to the Alpha Identity Provider base URL.");
        }

        // Empty audience turns audience validation off and leaves scope as the only gate.
        // The dev IDP emits a static audience of "{authority}/resources".
        var audience = identity["Audience"];
        var requireHttpsMetadata = identity.GetValue("RequireHttpsMetadata", true);
        var readScope = identity["ReadScope"] is { Length: > 0 } configured ? configured : DefaultReadScope;

        services.AddAuthentication()
            .AddJwtBearer(Scheme, options =>
            {
                options.Authority = authority;
                options.RequireHttpsMetadata = requireHttpsMetadata;
                options.MapInboundClaims = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = authority,
                    ValidateAudience = !string.IsNullOrWhiteSpace(audience),
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.FromSeconds(30)
                };
            });

        services.AddSingleton<IAuthorizationHandler, ProductApiScopeHandler>();
        services.AddAuthorizationBuilder()
            .AddPolicy(ReadPolicy, policy => policy
                .AddAuthenticationSchemes(Scheme)
                .RequireAuthenticatedUser()
                .AddRequirements(new ProductApiScopeRequirement(readScope)));

        return services;
    }
}
