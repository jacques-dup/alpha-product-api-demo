namespace Product.Domain;

public sealed record ProductMarket
{
    public Guid ProductId { get; init; }
    public string MarketCode { get; init; } = "";
    public DateOnly? LaunchedOn { get; init; }
}
