namespace Product.Domain;

public sealed record Language
{
    public string Code { get; init; } = "";
    public bool IsActive { get; init; } = true;
}
