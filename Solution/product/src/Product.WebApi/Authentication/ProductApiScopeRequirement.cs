using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Product.WebApi.Authentication;

/// <summary>
/// Requires a single OAuth scope on the caller's access token.
/// </summary>
/// <remarks>
/// Duende emits <c>scope</c> either as one claim per scope (default) or as a single
/// space-delimited claim. Both shapes are accepted here so the product does not depend
/// on IDP token-formatting options it must not change.
/// </remarks>
public sealed class ProductApiScopeRequirement(string scope) : IAuthorizationRequirement
{
    public const string ScopeClaimType = "scope";

    public string Scope { get; } = scope;
}

public sealed class ProductApiScopeHandler : AuthorizationHandler<ProductApiScopeRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ProductApiScopeRequirement requirement)
    {
        if (HasScope(context.User, requirement.Scope))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }

    internal static bool HasScope(ClaimsPrincipal user, string scope)
        => user.FindAll(ProductApiScopeRequirement.ScopeClaimType)
            .SelectMany(claim => claim.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .Any(granted => string.Equals(granted, scope, StringComparison.Ordinal));
}
