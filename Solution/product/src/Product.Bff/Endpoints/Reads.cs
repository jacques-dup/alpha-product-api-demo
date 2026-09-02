using Product.Domain;
using Product.WebApi.Endpoints;

namespace Product.Bff.Endpoints;

/// <summary>
/// Forwards to Product.WebApi GET handlers. Auth is applied on the BFF route group
/// (<c>alpha.idp.readwrite</c> + allow-list), not inside these methods.
/// Local wrappers exist so the ASP.NET route analyzer can see methods in this assembly.
/// </summary>
public static class ListLanguages
{
    public static Task<IResult> HandleAsync(IProductRepository repository, CancellationToken cancellationToken)
        => GetLanguages.HandleAsync(repository, cancellationToken);
}

public static class GetLanguageByCode
{
    public static Task<IResult> HandleAsync(string code, IProductRepository repository, CancellationToken cancellationToken)
        => GetLanguage.HandleAsync(code, repository, cancellationToken);
}

public static class ListMarkets
{
    public static Task<IResult> HandleAsync(IProductRepository repository, CancellationToken cancellationToken)
        => GetMarkets.HandleAsync(repository, cancellationToken);
}

public static class GetMarketByCode
{
    public static Task<IResult> HandleAsync(string code, IProductRepository repository, CancellationToken cancellationToken)
        => GetMarket.HandleAsync(code, repository, cancellationToken);
}

public static class ListFamilies
{
    public static Task<IResult> HandleAsync(IProductRepository repository, CancellationToken cancellationToken)
        => GetFamilies.HandleAsync(repository, cancellationToken);
}

public static class GetFamilyById
{
    public static Task<IResult> HandleAsync(Guid id, IProductRepository repository, CancellationToken cancellationToken)
        => GetFamily.HandleAsync(id, repository, cancellationToken);
}

public static class ListTags
{
    public static Task<IResult> HandleAsync(IProductRepository repository, CancellationToken cancellationToken)
        => GetTags.HandleAsync(repository, cancellationToken);
}

public static class GetTagById
{
    public static Task<IResult> HandleAsync(Guid id, IProductRepository repository, CancellationToken cancellationToken)
        => GetTag.HandleAsync(id, repository, cancellationToken);
}

public static class ListProducts
{
    public static Task<IResult> HandleAsync(
        IProductRepository repository,
        string? courseType,
        string? audience,
        string? country,
        string? language,
        CancellationToken cancellationToken)
        => GetProducts.HandleAsync(repository, courseType, audience, country, language, cancellationToken);
}

public static class GetProductByCode
{
    public static Task<IResult> HandleAsync(
        string code,
        IProductRepository repository,
        string? language,
        CancellationToken cancellationToken)
        => Product.WebApi.Endpoints.GetProductByCode.HandleAsync(code, repository, language, cancellationToken);
}

public static class GetProductById
{
    public static Task<IResult> HandleAsync(
        Guid id,
        IProductRepository repository,
        string? language,
        CancellationToken cancellationToken)
        => GetProduct.HandleAsync(id, repository, language, cancellationToken);
}

public static class ListItems
{
    public static Task<IResult> HandleAsync(
        IProductRepository repository,
        Guid? productId,
        CancellationToken cancellationToken)
        => GetProductItems.HandleAsync(repository, productId, cancellationToken);
}

public static class GetItemById
{
    public static Task<IResult> HandleAsync(Guid id, IProductRepository repository, CancellationToken cancellationToken)
        => GetProductItem.HandleAsync(id, repository, cancellationToken);
}

public static class ListAssets
{
    public static Task<IResult> HandleAsync(
        IProductRepository repository,
        Guid? productId,
        Guid? itemId,
        CancellationToken cancellationToken)
        => GetAssets.HandleAsync(repository, productId, itemId, cancellationToken);
}

public static class GetAssetById
{
    public static Task<IResult> HandleAsync(Guid id, IProductRepository repository, CancellationToken cancellationToken)
        => GetAsset.HandleAsync(id, repository, cancellationToken);
}

public static class ListProductTags
{
    public static Task<IResult> HandleAsync(
        IProductRepository repository,
        Guid? productId,
        CancellationToken cancellationToken)
        => GetProductTags.HandleAsync(repository, productId, cancellationToken);
}

public static class ListProductMarkets
{
    public static Task<IResult> HandleAsync(
        IProductRepository repository,
        Guid? productId,
        CancellationToken cancellationToken)
        => GetProductMarkets.HandleAsync(repository, productId, cancellationToken);
}

public static class ListAssetMarkets
{
    public static Task<IResult> HandleAsync(
        IProductRepository repository,
        Guid? assetId,
        CancellationToken cancellationToken)
        => GetAssetMarkets.HandleAsync(repository, assetId, cancellationToken);
}
