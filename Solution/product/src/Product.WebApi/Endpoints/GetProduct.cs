using Product.Domain;

namespace Product.WebApi.Endpoints;

public static class GetProduct
{
    public static async Task<IResult> HandleAsync(
        Guid id,
        IProductRepository repository,
        string? language,
        CancellationToken cancellationToken)
    {
        var snapshot = await ProductSnapshot.LoadAsync(repository, cancellationToken);
        var payload = ProductPayloadMapper.ToPayload(snapshot, id, language);
        return payload is null ? Results.NotFound() : Results.Ok(payload);
    }
}
