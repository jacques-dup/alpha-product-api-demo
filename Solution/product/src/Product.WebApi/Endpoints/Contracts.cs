namespace Product.WebApi.Endpoints;

public sealed record ProductMarketResponse(string MarketCode, DateOnly? LaunchedOn);

public sealed record AssetPayload(
    Guid Id,
    Guid? ItemId,
    string Role,
    string Kind,
    string? LanguageCode,
    string? Title,
    string? GroupCode,
    string Provider,
    string? ProviderAssetId,
    string? StreamUrl,
    string? DownloadUrl,
    bool AllowStream,
    bool AllowDownload,
    int? DurationSeconds,
    long? FileSizeBytes,
    IReadOnlyList<string> Markets);

public sealed record ProductItemPayload(
    Guid Id,
    string Kind,
    string Code,
    int Sequence,
    string Title,
    string? Summary,
    string? Grouping,
    bool IsOptional,
    IReadOnlyList<AssetPayload> Assets);

public sealed record ProductPayload(
    Guid Id,
    Guid FamilyId,
    string Code,
    string Title,
    string? Summary,
    string? Description,
    string ContentLanguage,
    Domain.ProductFamily Family,
    IReadOnlyList<Domain.Tag> Tags,
    IReadOnlyList<ProductMarketResponse> Markets,
    IReadOnlyList<ProductItemPayload> Items,
    IReadOnlyList<AssetPayload> ProductAssets);
