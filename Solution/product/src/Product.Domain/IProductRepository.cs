namespace Product.Domain;

/// <summary>
/// Persistence port for every catalog table. Implemented by Product.ProductStore.ProductRepository.
/// </summary>
public interface IProductRepository
{
    Task<IReadOnlyList<Language>> ListLanguagesAsync(CancellationToken cancellationToken = default);
    Task<Language?> GetLanguageAsync(string code, CancellationToken cancellationToken = default);
    Task<Language> AddLanguageAsync(Language language, CancellationToken cancellationToken = default);
    Task UpdateLanguageAsync(Language language, CancellationToken cancellationToken = default);
    Task DeleteLanguageAsync(string code, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Market>> ListMarketsAsync(CancellationToken cancellationToken = default);
    Task<Market?> GetMarketAsync(string code, CancellationToken cancellationToken = default);
    Task<Market> AddMarketAsync(Market market, CancellationToken cancellationToken = default);
    Task UpdateMarketAsync(Market market, CancellationToken cancellationToken = default);
    Task DeleteMarketAsync(string code, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProductFamily>> ListProductFamiliesAsync(CancellationToken cancellationToken = default);
    Task<ProductFamily?> GetProductFamilyAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ProductFamily> AddProductFamilyAsync(ProductFamily family, CancellationToken cancellationToken = default);
    Task UpdateProductFamilyAsync(ProductFamily family, CancellationToken cancellationToken = default);
    Task DeleteProductFamilyAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Product>> ListProductsAsync(CancellationToken cancellationToken = default);
    Task<Product?> GetProductAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Product> AddProductAsync(Product product, CancellationToken cancellationToken = default);
    Task UpdateProductAsync(Product product, CancellationToken cancellationToken = default);
    Task DeleteProductAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Tag>> ListTagsAsync(CancellationToken cancellationToken = default);
    Task<Tag?> GetTagAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Tag> AddTagAsync(Tag tag, CancellationToken cancellationToken = default);
    Task UpdateTagAsync(Tag tag, CancellationToken cancellationToken = default);
    Task DeleteTagAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProductTag>> ListProductTagsAsync(CancellationToken cancellationToken = default);
    Task<ProductTag?> GetProductTagAsync(Guid productId, Guid tagId, CancellationToken cancellationToken = default);
    Task<ProductTag> AddProductTagAsync(ProductTag productTag, CancellationToken cancellationToken = default);
    Task DeleteProductTagAsync(Guid productId, Guid tagId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProductMarket>> ListProductMarketsAsync(CancellationToken cancellationToken = default);
    Task<ProductMarket?> GetProductMarketAsync(Guid productId, string marketCode, CancellationToken cancellationToken = default);
    Task<ProductMarket> AddProductMarketAsync(ProductMarket productMarket, CancellationToken cancellationToken = default);
    Task UpdateProductMarketAsync(ProductMarket productMarket, CancellationToken cancellationToken = default);
    Task DeleteProductMarketAsync(Guid productId, string marketCode, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProductItem>> ListProductItemsAsync(CancellationToken cancellationToken = default);
    Task<ProductItem?> GetProductItemAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ProductItem> AddProductItemAsync(ProductItem item, CancellationToken cancellationToken = default);
    Task UpdateProductItemAsync(ProductItem item, CancellationToken cancellationToken = default);
    Task DeleteProductItemAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Asset>> ListAssetsAsync(CancellationToken cancellationToken = default);
    Task<Asset?> GetAssetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Asset> AddAssetAsync(Asset asset, CancellationToken cancellationToken = default);
    Task UpdateAssetAsync(Asset asset, CancellationToken cancellationToken = default);
    Task DeleteAssetAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AssetMarket>> ListAssetMarketsAsync(CancellationToken cancellationToken = default);
    Task<AssetMarket?> GetAssetMarketAsync(Guid assetId, string marketCode, CancellationToken cancellationToken = default);
    Task<AssetMarket> AddAssetMarketAsync(AssetMarket assetMarket, CancellationToken cancellationToken = default);
    Task DeleteAssetMarketAsync(Guid assetId, string marketCode, CancellationToken cancellationToken = default);
}
