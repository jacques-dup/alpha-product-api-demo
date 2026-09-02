namespace Product.Domain;

/// <summary>
/// Requested language selects assets; omitted language falls back to the product's content language.
/// Language-neutral assets (null <see cref="Asset.LanguageCode"/>) are always included.
/// </summary>
public static class AssetLanguageSelector
{
    public static IReadOnlyList<Asset> Select(
        IEnumerable<Asset> assets,
        string contentLanguage,
        string? requestedLanguage)
    {
        ArgumentNullException.ThrowIfNull(assets);

        var language = string.IsNullOrWhiteSpace(requestedLanguage)
            ? contentLanguage
            : requestedLanguage.Trim();

        return assets
            .Where(asset =>
                asset.LanguageCode is null
                || string.Equals(asset.LanguageCode, language, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}
