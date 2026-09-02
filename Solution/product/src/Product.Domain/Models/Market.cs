namespace Product.Domain;

public sealed record Market
{
    public string Code { get; init; } = "";
    public string Kind { get; init; } = "";
    public string Name { get; init; } = "";
}
