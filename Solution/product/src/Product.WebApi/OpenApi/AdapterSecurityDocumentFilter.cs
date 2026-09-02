using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Product.WebApi.OpenApi;

/// <summary>
/// Bearer on the Product API document; CSRF header on the portal BFF document.
/// Cookie session for BFF Try it out comes from <c>/bff/login</c>, not Authorize.
/// </summary>
internal sealed class AdapterSecurityDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        swaggerDoc.SecurityRequirements.Clear();

        if (context.DocumentName == ProductOpenApi.DocumentName)
        {
            swaggerDoc.SecurityRequirements.Add(Requirement(ProductOpenApi.BearerSchemeId));
        }
        else if (context.DocumentName == ProductOpenApi.BffDocumentName)
        {
            swaggerDoc.SecurityRequirements.Add(Requirement(ProductOpenApi.CsrfSchemeId));
        }
    }

    private static OpenApiSecurityRequirement Requirement(string schemeId)
        => new()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = schemeId
                    }
                },
                Array.Empty<string>()
            }
        };
}
