using Product.Domain;

namespace Product.WebApi.Endpoints;

public static class GetAsset
{
    public static async Task<IResult> HandleAsync(
        Guid id,
        IProductRepository repository,
        CancellationToken cancellationToken)
    {
        var asset = await repository.GetAssetAsync(id, cancellationToken);
        return asset is null ? Results.NotFound() : Results.Ok(asset);
    }
}
