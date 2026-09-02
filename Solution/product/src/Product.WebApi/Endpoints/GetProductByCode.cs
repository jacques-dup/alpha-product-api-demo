using Product.Domain;

namespace Product.WebApi.Endpoints;

public static class GetProductByCode
{
    public static async Task<IResult> HandleAsync(
        string code,
        IProductRepository repository,
        string? language,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return Results.Problem(statusCode: StatusCodes.Status400BadRequest, detail: "Product code is required.");
        }

        var snapshot = await ProductSnapshot.LoadAsync(repository, cancellationToken);
        var payload = ProductPayloadMapper.ToPayload(snapshot, code, language);
        return payload is null ? Results.NotFound() : Results.Ok(payload);
    }
}
