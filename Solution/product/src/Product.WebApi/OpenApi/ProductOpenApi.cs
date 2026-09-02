using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Product.WebApi.Authentication;

namespace Product.WebApi.OpenApi;

/// <summary>
/// Swagger / OpenAPI for humans. Two documents on the same host:
/// Product API (<c>/product</c>, bearer) and portal BFF (<c>/api</c>, cookie + CSRF).
/// </summary>
public static class ProductOpenApi
{
    public const string RoutePrefix = "swagger";
    public const string DocumentName = "v1";
    public const string BffDocumentName = "bff";
    public const string BearerSchemeId = "Bearer";
    public const string CsrfSchemeId = "Csrf";

    public static IServiceCollection AddProductOpenApi(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var authority = configuration["Identity:Authority"]?.TrimEnd('/');
        var readScope = configuration["Identity:ReadScope"] is { Length: > 0 } configured
            ? configured
            : ProductApiAuthentication.DefaultReadScope;

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(DocumentName, new OpenApiInfo
            {
                Title = "Product API",
                Version = DocumentName,
                Description =
                    "Read-only product data. Authenticate with a client-credentials access token " +
                    $"from the Alpha Identity Provider (scope `{readScope}`). " +
                    "Click Authorize and paste the token — get one from `products.http` or POST " +
                    $"{authority}/connect/token. There is no end-user login on this API."
            });

            options.SwaggerDoc(BffDocumentName, new OpenApiInfo
            {
                Title = "Portal BFF",
                Version = BffDocumentName,
                Description =
                    "Staff CRUD for the admin portal. Authenticate via `/bff/login` (OIDC cookie, " +
                    "scope `alpha.idp.readwrite`, allow-list). Try it out sends the cookie on this origin; " +
                    "Authorize sets `X-CSRF: 1` (required by Duende BFF). Not a client-credentials API."
            });

            options.AddSecurityDefinition(BearerSchemeId, new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = $"Client-credentials access token (scope {readScope}). Product API only."
            });

            options.AddSecurityDefinition(CsrfSchemeId, new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Name = "X-CSRF",
                Description = "Duende BFF anti-forgery. Value must be `1`. Log in at `/bff/login` first so the portal cookie is sent."
            });

            options.DocInclusionPredicate((documentName, api) =>
            {
                if (!string.IsNullOrEmpty(api.GroupName))
                {
                    return string.Equals(api.GroupName, documentName, StringComparison.OrdinalIgnoreCase);
                }

                var path = api.RelativePath ?? "";
                return documentName == DocumentName
                    ? path.StartsWith("product", StringComparison.OrdinalIgnoreCase)
                    : documentName == BffDocumentName
                        && path.StartsWith("api", StringComparison.OrdinalIgnoreCase);
            });

            options.DocumentFilter<AdapterSecurityDocumentFilter>();
        });

        return services;
    }

    public static WebApplication UseProductOpenApi(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint($"/{RoutePrefix}/{DocumentName}/swagger.json", "Product API");
            options.SwaggerEndpoint($"/{RoutePrefix}/{BffDocumentName}/swagger.json", "Portal BFF");
            options.RoutePrefix = RoutePrefix;
            options.DocumentTitle = "Product";
            options.EnablePersistAuthorization();
            options.DisplayRequestDuration();
            options.ConfigObject.AdditionalItems["withCredentials"] = true;
        });

        return app;
    }
}
