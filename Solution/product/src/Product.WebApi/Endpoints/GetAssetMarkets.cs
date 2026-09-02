using Product.Domain;

namespace Product.WebApi.Endpoints;

public static class GetAssetMarkets
{
    public static async Task<IResult> HandleAsync(
        IProductRepository repository,
        Guid? assetId,
        CancellationToken cancellationToken)
    {
        var links = await repository.ListAssetMarketsAsync(cancellationToken);
        if (assetId is { } id)
        {
            links = links.Where(link => link.AssetId == id).ToList();
        }

        return Results.Ok(links);
    }
}
