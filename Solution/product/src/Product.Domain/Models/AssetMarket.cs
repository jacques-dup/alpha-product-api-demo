namespace Product.Domain;

public sealed record AssetMarket
{
    public Guid AssetId { get; init; }
    public string MarketCode { get; init; } = "";
}
