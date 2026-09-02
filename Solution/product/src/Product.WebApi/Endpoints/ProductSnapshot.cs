using Product.Domain;

namespace Product.WebApi.Endpoints;

internal sealed record ProductSnapshot
{
    public required IReadOnlyList<Domain.Product> Products { get; init; }
    public required IReadOnlyList<ProductFamily> Families { get; init; }
    public required IReadOnlyList<Tag> Tags { get; init; }
    public required IReadOnlyList<ProductTag> ProductTags { get; init; }
    public required IReadOnlyList<ProductMarket> ProductMarkets { get; init; }
    public required IReadOnlyList<ProductItem> Items { get; init; }
    public required IReadOnlyList<Asset> Assets { get; init; }
    public required IReadOnlyList<AssetMarket> AssetMarkets { get; init; }

    public static async Task<ProductSnapshot> LoadAsync(
        IProductRepository repository,
        CancellationToken cancellationToken)
    {
        return new ProductSnapshot
        {
            Products = await repository.ListProductsAsync(cancellationToken),
            Families = await repository.ListProductFamiliesAsync(cancellationToken),
            Tags = await repository.ListTagsAsync(cancellationToken),
            ProductTags = await repository.ListProductTagsAsync(cancellationToken),
            ProductMarkets = await repository.ListProductMarketsAsync(cancellationToken),
            Items = await repository.ListProductItemsAsync(cancellationToken),
            Assets = await repository.ListAssetsAsync(cancellationToken),
            AssetMarkets = await repository.ListAssetMarketsAsync(cancellationToken)
        };
    }
}
