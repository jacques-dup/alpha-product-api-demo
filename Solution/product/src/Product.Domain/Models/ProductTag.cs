namespace Product.Domain;

public sealed record ProductTag
{
    public Guid ProductId { get; init; }
    public Guid TagId { get; init; }
}
