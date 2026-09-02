namespace Product.Domain;

public sealed record ProductFamily
{
    public Guid Id { get; init; }
    public string Code { get; init; } = "";
    public string Name { get; init; } = "";
    public string? Summary { get; init; }
    public int Sequence { get; init; }
}
