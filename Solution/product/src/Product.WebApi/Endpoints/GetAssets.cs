using Product.Domain;

namespace Product.WebApi.Endpoints;

public static class GetAssets
{
    public static async Task<IResult> HandleAsync(
        IProductRepository repository,
        Guid? productId,
        Guid? itemId,
        CancellationToken cancellationToken)
    {
        var assets = await repository.ListAssetsAsync(cancellationToken);
        if (productId is { } product)
        {
            assets = assets.Where(asset => asset.ProductId == product).ToList();
        }

        if (itemId is { } item)
        {
            assets = assets.Where(asset => asset.ItemId == item).ToList();
        }

        return Results.Ok(assets);
    }
}
