namespace Product.Domain;

public sealed record Tag
{
    public Guid Id { get; init; }
    public string Category { get; init; } = "";
    public string Code { get; init; } = "";
    public string Name { get; init; } = "";
    public bool IsPublic { get; init; } = true;
    public int Sequence { get; init; }
}
