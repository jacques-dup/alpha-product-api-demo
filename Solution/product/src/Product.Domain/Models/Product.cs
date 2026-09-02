namespace Product.Domain;

public sealed record Product
{
    public Guid Id { get; init; }
    public Guid FamilyId { get; init; }
    public string Code { get; init; } = "";
    public string Title { get; init; } = "";
    public string? Summary { get; init; }
    public string? Description { get; init; }
    public string ContentLanguage { get; init; } = "";
}
