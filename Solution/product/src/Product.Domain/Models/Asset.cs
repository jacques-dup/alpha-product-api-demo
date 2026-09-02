namespace Product.Domain;

public sealed record Asset
{
    public Guid Id { get; init; }
    public Guid ProductId { get; init; }
    public Guid? ItemId { get; init; }
    public string Role { get; init; } = "";
    public string Kind { get; init; } = "";
    public string? LanguageCode { get; init; }
    public string? Title { get; init; }
    public string? GroupCode { get; init; }
    public string Provider { get; init; } = "";
    public string? ProviderAssetId { get; init; }
    public string? StreamUrl { get; init; }
    public string? DownloadUrl { get; init; }
    public bool AllowStream { get; init; }
    public bool AllowDownload { get; init; }
    public int? DurationSeconds { get; init; }
    public long? FileSizeBytes { get; init; }
}
