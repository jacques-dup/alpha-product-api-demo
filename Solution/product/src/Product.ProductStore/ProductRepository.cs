using Microsoft.EntityFrameworkCore;
using Product.Domain;

namespace Product.ProductStore;

public sealed class ProductRepository : IProductRepository
{
    private readonly ProductDbContext _db;

    public ProductRepository(ProductDbContext db)
    {
        _db = db;
    }

    public Task<IReadOnlyList<Language>> ListLanguagesAsync(CancellationToken cancellationToken = default)
        => ListAsync(_db.Languages.OrderBy(x => x.Code), cancellationToken);

    public Task<Language?> GetLanguageAsync(string code, CancellationToken cancellationToken = default)
        => _db.Languages.AsNoTracking().FirstOrDefaultAsync(x => x.Code == code, cancellationToken);

    public Task<Language> AddLanguageAsync(Language language, CancellationToken cancellationToken = default)
        => AddAsync(_db.Languages, language, cancellationToken);

    public Task UpdateLanguageAsync(Language language, CancellationToken cancellationToken = default)
        => UpdateAsync(language, cancellationToken);

    public Task DeleteLanguageAsync(string code, CancellationToken cancellationToken = default)
        => DeleteAsync<Language>(code, cancellationToken);

    public Task<IReadOnlyList<Market>> ListMarketsAsync(CancellationToken cancellationToken = default)
        => ListAsync(_db.Markets.OrderBy(x => x.Code), cancellationToken);

    public Task<Market?> GetMarketAsync(string code, CancellationToken cancellationToken = default)
        => _db.Markets.AsNoTracking().FirstOrDefaultAsync(x => x.Code == code, cancellationToken);

    public Task<Market> AddMarketAsync(Market market, CancellationToken cancellationToken = default)
        => AddAsync(_db.Markets, market, cancellationToken);

    public Task UpdateMarketAsync(Market market, CancellationToken cancellationToken = default)
        => UpdateAsync(market, cancellationToken);

    public Task DeleteMarketAsync(string code, CancellationToken cancellationToken = default)
        => DeleteAsync<Market>(code, cancellationToken);

    public Task<IReadOnlyList<ProductFamily>> ListProductFamiliesAsync(CancellationToken cancellationToken = default)
        => ListAsync(_db.ProductFamilies.OrderBy(x => x.Sequence).ThenBy(x => x.Code), cancellationToken);

    public Task<ProductFamily?> GetProductFamilyAsync(Guid id, CancellationToken cancellationToken = default)
        => _db.ProductFamilies.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<ProductFamily> AddProductFamilyAsync(ProductFamily family, CancellationToken cancellationToken = default)
        => AddAsync(_db.ProductFamilies, family with { Id = NewId(family.Id) }, cancellationToken);

    public Task UpdateProductFamilyAsync(ProductFamily family, CancellationToken cancellationToken = default)
        => UpdateAsync(family, cancellationToken);

    public Task DeleteProductFamilyAsync(Guid id, CancellationToken cancellationToken = default)
        => DeleteAsync<ProductFamily>(id, cancellationToken);

    public Task<IReadOnlyList<Domain.Product>> ListProductsAsync(CancellationToken cancellationToken = default)
        => ListAsync(_db.Products.OrderBy(x => x.Code), cancellationToken);

    public Task<Domain.Product?> GetProductAsync(Guid id, CancellationToken cancellationToken = default)
        => _db.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<Domain.Product> AddProductAsync(Domain.Product product, CancellationToken cancellationToken = default)
        => AddAsync(_db.Products, product with { Id = NewId(product.Id) }, cancellationToken);

    public Task UpdateProductAsync(Domain.Product product, CancellationToken cancellationToken = default)
        => UpdateAsync(product, cancellationToken);

    public Task DeleteProductAsync(Guid id, CancellationToken cancellationToken = default)
        => DeleteAsync<Domain.Product>(id, cancellationToken);

    public Task<IReadOnlyList<Tag>> ListTagsAsync(CancellationToken cancellationToken = default)
        => ListAsync(_db.Tags.OrderBy(x => x.Category).ThenBy(x => x.Sequence).ThenBy(x => x.Code), cancellationToken);

    public Task<Tag?> GetTagAsync(Guid id, CancellationToken cancellationToken = default)
        => _db.Tags.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<Tag> AddTagAsync(Tag tag, CancellationToken cancellationToken = default)
        => AddAsync(_db.Tags, tag with { Id = NewId(tag.Id) }, cancellationToken);

    public Task UpdateTagAsync(Tag tag, CancellationToken cancellationToken = default)
        => UpdateAsync(tag, cancellationToken);

    public Task DeleteTagAsync(Guid id, CancellationToken cancellationToken = default)
        => DeleteAsync<Tag>(id, cancellationToken);

    public Task<IReadOnlyList<ProductTag>> ListProductTagsAsync(CancellationToken cancellationToken = default)
        => ListAsync(_db.ProductTags.OrderBy(x => x.ProductId).ThenBy(x => x.TagId), cancellationToken);

    public Task<ProductTag?> GetProductTagAsync(Guid productId, Guid tagId, CancellationToken cancellationToken = default)
        => _db.ProductTags.AsNoTracking().FirstOrDefaultAsync(x => x.ProductId == productId && x.TagId == tagId, cancellationToken);

    public Task<ProductTag> AddProductTagAsync(ProductTag productTag, CancellationToken cancellationToken = default)
        => AddAsync(_db.ProductTags, productTag, cancellationToken);

    public Task DeleteProductTagAsync(Guid productId, Guid tagId, CancellationToken cancellationToken = default)
        => DeleteAsync<ProductTag>(new object[] { productId, tagId }, cancellationToken);

    public Task<IReadOnlyList<ProductMarket>> ListProductMarketsAsync(CancellationToken cancellationToken = default)
        => ListAsync(_db.ProductMarkets.OrderBy(x => x.ProductId).ThenBy(x => x.MarketCode), cancellationToken);

    public Task<ProductMarket?> GetProductMarketAsync(Guid productId, string marketCode, CancellationToken cancellationToken = default)
        => _db.ProductMarkets.AsNoTracking().FirstOrDefaultAsync(x => x.ProductId == productId && x.MarketCode == marketCode, cancellationToken);

    public Task<ProductMarket> AddProductMarketAsync(ProductMarket productMarket, CancellationToken cancellationToken = default)
        => AddAsync(_db.ProductMarkets, productMarket, cancellationToken);

    public Task UpdateProductMarketAsync(ProductMarket productMarket, CancellationToken cancellationToken = default)
        => UpdateAsync(productMarket, cancellationToken);

    public Task DeleteProductMarketAsync(Guid productId, string marketCode, CancellationToken cancellationToken = default)
        => DeleteAsync<ProductMarket>(new object[] { productId, marketCode }, cancellationToken);

    public Task<IReadOnlyList<ProductItem>> ListProductItemsAsync(CancellationToken cancellationToken = default)
        => ListAsync(_db.ProductItems.OrderBy(x => x.ProductId).ThenBy(x => x.Kind).ThenBy(x => x.Sequence), cancellationToken);

    public Task<ProductItem?> GetProductItemAsync(Guid id, CancellationToken cancellationToken = default)
        => _db.ProductItems.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<ProductItem> AddProductItemAsync(ProductItem item, CancellationToken cancellationToken = default)
        => AddAsync(_db.ProductItems, item with { Id = NewId(item.Id) }, cancellationToken);

    public Task UpdateProductItemAsync(ProductItem item, CancellationToken cancellationToken = default)
        => UpdateAsync(item, cancellationToken);

    public Task DeleteProductItemAsync(Guid id, CancellationToken cancellationToken = default)
        => DeleteAsync<ProductItem>(id, cancellationToken);

    public Task<IReadOnlyList<Asset>> ListAssetsAsync(CancellationToken cancellationToken = default)
        => ListAsync(_db.Assets.OrderBy(x => x.ProductId).ThenBy(x => x.Role), cancellationToken);

    public Task<Asset?> GetAssetAsync(Guid id, CancellationToken cancellationToken = default)
        => _db.Assets.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<Asset> AddAssetAsync(Asset asset, CancellationToken cancellationToken = default)
        => AddAsync(_db.Assets, asset with { Id = NewId(asset.Id) }, cancellationToken);

    public Task UpdateAssetAsync(Asset asset, CancellationToken cancellationToken = default)
        => UpdateAsync(asset, cancellationToken);

    public Task DeleteAssetAsync(Guid id, CancellationToken cancellationToken = default)
        => DeleteAsync<Asset>(id, cancellationToken);

    public Task<IReadOnlyList<AssetMarket>> ListAssetMarketsAsync(CancellationToken cancellationToken = default)
        => ListAsync(_db.AssetMarkets.OrderBy(x => x.AssetId).ThenBy(x => x.MarketCode), cancellationToken);

    public Task<AssetMarket?> GetAssetMarketAsync(Guid assetId, string marketCode, CancellationToken cancellationToken = default)
        => _db.AssetMarkets.AsNoTracking().FirstOrDefaultAsync(x => x.AssetId == assetId && x.MarketCode == marketCode, cancellationToken);

    public Task<AssetMarket> AddAssetMarketAsync(AssetMarket assetMarket, CancellationToken cancellationToken = default)
        => AddAsync(_db.AssetMarkets, assetMarket, cancellationToken);

    public Task DeleteAssetMarketAsync(Guid assetId, string marketCode, CancellationToken cancellationToken = default)
        => DeleteAsync<AssetMarket>(new object[] { assetId, marketCode }, cancellationToken);

    private static Guid NewId(Guid id) => id == Guid.Empty ? Guid.NewGuid() : id;

    private static async Task<IReadOnlyList<T>> ListAsync<T>(IQueryable<T> query, CancellationToken cancellationToken)
        where T : class
        => await query.AsNoTracking().ToListAsync(cancellationToken);

    private async Task<T> AddAsync<T>(DbSet<T> set, T entity, CancellationToken cancellationToken)
        where T : class
    {
        set.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return entity;
    }

    private Task UpdateAsync<T>(T entity, CancellationToken cancellationToken)
        where T : class
    {
        _db.Update(entity);
        return _db.SaveChangesAsync(cancellationToken);
    }

    private async Task DeleteAsync<T>(object key, CancellationToken cancellationToken)
        where T : class
    {
        var entity = await _db.Set<T>().FindAsync([key], cancellationToken);
        if (entity is null)
        {
            return;
        }

        _db.Set<T>().Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }

    private async Task DeleteAsync<T>(object[] key, CancellationToken cancellationToken)
        where T : class
    {
        var entity = await _db.Set<T>().FindAsync(key, cancellationToken);
        if (entity is null)
        {
            return;
        }

        _db.Set<T>().Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
