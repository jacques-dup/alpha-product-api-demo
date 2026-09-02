using Product.Domain;

namespace Product.WebApi.Endpoints;

internal static class ProductPayloadMapper
{
    public static ProductPayload? ToPayload(ProductSnapshot snapshot, Guid productId, string? language)
    {
        var product = snapshot.Products.FirstOrDefault(p => p.Id == productId);
        return product is null ? null : ToPayload(snapshot, product, language);
    }

    public static ProductPayload? ToPayload(ProductSnapshot snapshot, string code, string? language)
    {
        var product = snapshot.Products.FirstOrDefault(p =>
            string.Equals(p.Code, code, StringComparison.OrdinalIgnoreCase));
        return product is null ? null : ToPayload(snapshot, product, language);
    }

    public static IReadOnlyList<ProductPayload> ListPayloads(
        ProductSnapshot snapshot,
        string? courseType,
        string? audience,
        string? marketCode,
        string? language)
    {
        var products = snapshot.Products.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(courseType))
        {
            products = products.Where(product => MatchesCourseType(snapshot, product, courseType));
        }

        if (!string.IsNullOrWhiteSpace(audience))
        {
            products = products.Where(product => HasAudience(snapshot, product.Id, audience));
        }

        if (!string.IsNullOrWhiteSpace(marketCode))
        {
            products = products.Where(product =>
                snapshot.ProductMarkets.Any(link =>
                    link.ProductId == product.Id
                    && string.Equals(link.MarketCode, marketCode, StringComparison.OrdinalIgnoreCase)));
        }

        return products.Select(product => ToPayload(snapshot, product, language)).ToList();
    }

    public static ProductPayload ToPayload(ProductSnapshot snapshot, Domain.Product product, string? language)
    {
        var family = snapshot.Families.FirstOrDefault(f => f.Id == product.FamilyId)
            ?? new ProductFamily { Id = product.FamilyId, Code = "", Name = "" };

        var tagIds = snapshot.ProductTags
            .Where(link => link.ProductId == product.Id)
            .Select(link => link.TagId)
            .ToHashSet();

        var tags = snapshot.Tags.Where(tag => tagIds.Contains(tag.Id)).ToList();

        var markets = snapshot.ProductMarkets
            .Where(link => link.ProductId == product.Id)
            .Select(link => new ProductMarketResponse(link.MarketCode, link.LaunchedOn))
            .ToList();

        var selectedAssets = AssetLanguageSelector.Select(
            snapshot.Assets.Where(asset => asset.ProductId == product.Id),
            product.ContentLanguage,
            language);

        var items = snapshot.Items
            .Where(item => item.ProductId == product.Id)
            .OrderBy(item => item.Kind)
            .ThenBy(item => item.Sequence)
            .Select(item => new ProductItemPayload(
                item.Id,
                item.Kind,
                item.Code,
                item.Sequence,
                item.Title,
                item.Summary,
                item.Grouping,
                item.IsOptional,
                selectedAssets.Where(asset => asset.ItemId == item.Id).Select(ToAsset).ToList()))
            .ToList();

        var productAssets = selectedAssets
            .Where(asset => asset.ItemId is null)
            .Select(ToAsset)
            .ToList();

        return new ProductPayload(
            product.Id,
            product.FamilyId,
            product.Code,
            product.Title,
            product.Summary,
            product.Description,
            product.ContentLanguage,
            family,
            tags,
            markets,
            items,
            productAssets);

        AssetPayload ToAsset(Asset asset) => new(
            asset.Id,
            asset.ItemId,
            asset.Role,
            asset.Kind,
            asset.LanguageCode,
            asset.Title,
            asset.GroupCode,
            asset.Provider,
            asset.ProviderAssetId,
            asset.StreamUrl,
            asset.DownloadUrl,
            asset.AllowStream,
            asset.AllowDownload,
            asset.DurationSeconds,
            asset.FileSizeBytes,
            snapshot.AssetMarkets
                .Where(link => link.AssetId == asset.Id)
                .Select(link => link.MarketCode)
                .ToList());
    }

    private static bool MatchesCourseType(ProductSnapshot snapshot, Domain.Product product, string courseType)
    {
        if (product.Code.Equals(courseType, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        var family = snapshot.Families.FirstOrDefault(f => f.Id == product.FamilyId);
        return family is not null
            && family.Code.Equals(courseType, StringComparison.OrdinalIgnoreCase);
    }

    private static bool HasAudience(ProductSnapshot snapshot, Guid productId, string audience)
    {
        var tagIds = snapshot.ProductTags
            .Where(link => link.ProductId == productId)
            .Select(link => link.TagId)
            .ToHashSet();

        return snapshot.Tags.Any(tag =>
            tagIds.Contains(tag.Id)
            && tag.Category.Equals("audience", StringComparison.OrdinalIgnoreCase)
            && tag.Code.Equals(audience, StringComparison.OrdinalIgnoreCase));
    }
}
