using Product.Domain;

namespace Product.WebApi.Endpoints;

public static class GetProducts
{
    public static async Task<IResult> HandleAsync(
        IProductRepository repository,
        string? courseType,
        string? audience,
        string? country,
        string? language,
        CancellationToken cancellationToken)
    {
        string? marketCode = null;
        if (!string.IsNullOrWhiteSpace(country))
        {
            if (!CountryMarketAcl.TryResolve(country, out marketCode))
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    detail: $"Unknown country or market '{country}'.");
            }
        }

        var snapshot = await ProductSnapshot.LoadAsync(repository, cancellationToken);
        var payloads = ProductPayloadMapper.ListPayloads(snapshot, courseType, audience, marketCode, language);
        return Results.Ok(payloads);
    }
}
