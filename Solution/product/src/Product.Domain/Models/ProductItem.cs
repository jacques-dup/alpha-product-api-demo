namespace Product.Domain;

public sealed record ProductItem
{
    public Guid Id { get; init; }
    public Guid ProductId { get; init; }
    public string Kind { get; init; } = "";
    public string Code { get; init; } = "";
    public int Sequence { get; init; }
    public string Title { get; init; } = "";
    public string? Summary { get; init; }
    public string? Grouping { get; init; }
    public bool IsOptional { get; init; }
}
