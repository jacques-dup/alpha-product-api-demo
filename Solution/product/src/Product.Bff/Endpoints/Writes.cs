using Product.Domain;

namespace Product.Bff.Endpoints;

public static class CreateLanguage
{
    public static async Task<IResult> HandleAsync(Language language, IProductRepository repository, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(language.Code))
        {
            return Results.Problem(statusCode: StatusCodes.Status400BadRequest, detail: "Language code is required.");
        }

        var created = await repository.AddLanguageAsync(language, cancellationToken);
        return Results.Created($"{ProductBffAdapter.RoutePrefix}/languages/{created.Code}", created);
    }
}

public static class UpdateLanguage
{
    public static async Task<IResult> HandleAsync(string code, Language language, IProductRepository repository, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return Results.Problem(statusCode: StatusCodes.Status400BadRequest, detail: "Language code is required.");
        }

        var existing = await repository.GetLanguageAsync(code, cancellationToken);
        if (existing is null)
        {
            return Results.NotFound();
        }

        await repository.UpdateLanguageAsync(language with { Code = code }, cancellationToken);
        return Results.Ok(language with { Code = code });
    }
}

public static class DeleteLanguage
{
    public static async Task<IResult> HandleAsync(string code, IProductRepository repository, CancellationToken cancellationToken)
    {
        var existing = await repository.GetLanguageAsync(code, cancellationToken);
        if (existing is null)
        {
            return Results.NotFound();
        }

        await repository.DeleteLanguageAsync(code, cancellationToken);
        return Results.NoContent();
    }
}

public static class CreateMarket
{
    public static async Task<IResult> HandleAsync(Market market, IProductRepository repository, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(market.Code))
        {
            return Results.Problem(statusCode: StatusCodes.Status400BadRequest, detail: "Market code is required.");
        }

        var created = await repository.AddMarketAsync(market, cancellationToken);
        return Results.Created($"{ProductBffAdapter.RoutePrefix}/markets/{created.Code}", created);
    }
}

public static class UpdateMarket
{
    public static async Task<IResult> HandleAsync(string code, Market market, IProductRepository repository, CancellationToken cancellationToken)
    {
        var existing = await repository.GetMarketAsync(code, cancellationToken);
        if (existing is null)
        {
            return Results.NotFound();
        }

        await repository.UpdateMarketAsync(market with { Code = code }, cancellationToken);
        return Results.Ok(market with { Code = code });
    }
}

public static class DeleteMarket
{
    public static async Task<IResult> HandleAsync(string code, IProductRepository repository, CancellationToken cancellationToken)
    {
        var existing = await repository.GetMarketAsync(code, cancellationToken);
        if (existing is null)
        {
            return Results.NotFound();
        }

        await repository.DeleteMarketAsync(code, cancellationToken);
        return Results.NoContent();
    }
}

public static class CreateFamily
{
    public static async Task<IResult> HandleAsync(ProductFamily family, IProductRepository repository, CancellationToken cancellationToken)
    {
        var created = await repository.AddProductFamilyAsync(family, cancellationToken);
        return Results.Created($"{ProductBffAdapter.RoutePrefix}/families/{created.Id}", created);
    }
}

public static class UpdateFamily
{
    public static async Task<IResult> HandleAsync(Guid id, ProductFamily family, IProductRepository repository, CancellationToken cancellationToken)
    {
        var existing = await repository.GetProductFamilyAsync(id, cancellationToken);
        if (existing is null)
        {
            return Results.NotFound();
        }

        var updated = family with { Id = id };
        await repository.UpdateProductFamilyAsync(updated, cancellationToken);
        return Results.Ok(updated);
    }
}

public static class DeleteFamily
{
    public static async Task<IResult> HandleAsync(Guid id, IProductRepository repository, CancellationToken cancellationToken)
    {
        var existing = await repository.GetProductFamilyAsync(id, cancellationToken);
        if (existing is null)
        {
            return Results.NotFound();
        }

        await repository.DeleteProductFamilyAsync(id, cancellationToken);
        return Results.NoContent();
    }
}

public static class CreateTag
{
    public static async Task<IResult> HandleAsync(Tag tag, IProductRepository repository, CancellationToken cancellationToken)
    {
        var created = await repository.AddTagAsync(tag, cancellationToken);
        return Results.Created($"{ProductBffAdapter.RoutePrefix}/tags/{created.Id}", created);
    }
}

public static class UpdateTag
{
    public static async Task<IResult> HandleAsync(Guid id, Tag tag, IProductRepository repository, CancellationToken cancellationToken)
    {
        var existing = await repository.GetTagAsync(id, cancellationToken);
        if (existing is null)
        {
            return Results.NotFound();
        }

        var updated = tag with { Id = id };
        await repository.UpdateTagAsync(updated, cancellationToken);
        return Results.Ok(updated);
    }
}

public static class DeleteTag
{
    public static async Task<IResult> HandleAsync(Guid id, IProductRepository repository, CancellationToken cancellationToken)
    {
        var existing = await repository.GetTagAsync(id, cancellationToken);
        if (existing is null)
        {
            return Results.NotFound();
        }

        await repository.DeleteTagAsync(id, cancellationToken);
        return Results.NoContent();
    }
}

public static class CreateProduct
{
    public static async Task<IResult> HandleAsync(Domain.Product product, IProductRepository repository, CancellationToken cancellationToken)
    {
        var created = await repository.AddProductAsync(product, cancellationToken);
        return Results.Created($"{ProductBffAdapter.RoutePrefix}/products/{created.Id}", created);
    }
}

public static class UpdateProduct
{
    public static async Task<IResult> HandleAsync(Guid id, Domain.Product product, IProductRepository repository, CancellationToken cancellationToken)
    {
        var existing = await repository.GetProductAsync(id, cancellationToken);
        if (existing is null)
        {
            return Results.NotFound();
        }

        var updated = product with { Id = id };
        await repository.UpdateProductAsync(updated, cancellationToken);
        return Results.Ok(updated);
    }
}

public static class DeleteProduct
{
    public static async Task<IResult> HandleAsync(Guid id, IProductRepository repository, CancellationToken cancellationToken)
    {
        var existing = await repository.GetProductAsync(id, cancellationToken);
        if (existing is null)
        {
            return Results.NotFound();
        }

        await repository.DeleteProductAsync(id, cancellationToken);
        return Results.NoContent();
    }
}

public static class CreateItem
{
    public static async Task<IResult> HandleAsync(ProductItem item, IProductRepository repository, CancellationToken cancellationToken)
    {
        var created = await repository.AddProductItemAsync(item, cancellationToken);
        return Results.Created($"{ProductBffAdapter.RoutePrefix}/items/{created.Id}", created);
    }
}

public static class UpdateItem
{
    public static async Task<IResult> HandleAsync(Guid id, ProductItem item, IProductRepository repository, CancellationToken cancellationToken)
    {
        var existing = await repository.GetProductItemAsync(id, cancellationToken);
        if (existing is null)
        {
            return Results.NotFound();
        }

        var updated = item with { Id = id };
        await repository.UpdateProductItemAsync(updated, cancellationToken);
        return Results.Ok(updated);
    }
}

public static class DeleteItem
{
    public static async Task<IResult> HandleAsync(Guid id, IProductRepository repository, CancellationToken cancellationToken)
    {
        var existing = await repository.GetProductItemAsync(id, cancellationToken);
        if (existing is null)
        {
            return Results.NotFound();
        }

        await repository.DeleteProductItemAsync(id, cancellationToken);
        return Results.NoContent();
    }
}

public static class CreateAsset
{
    public static async Task<IResult> HandleAsync(Asset asset, IProductRepository repository, CancellationToken cancellationToken)
    {
        var created = await repository.AddAssetAsync(asset, cancellationToken);
        return Results.Created($"{ProductBffAdapter.RoutePrefix}/assets/{created.Id}", created);
    }
}

public static class UpdateAsset
{
    public static async Task<IResult> HandleAsync(Guid id, Asset asset, IProductRepository repository, CancellationToken cancellationToken)
    {
        var existing = await repository.GetAssetAsync(id, cancellationToken);
        if (existing is null)
        {
            return Results.NotFound();
        }

        var updated = asset with { Id = id };
        await repository.UpdateAssetAsync(updated, cancellationToken);
        return Results.Ok(updated);
    }
}

public static class DeleteAsset
{
    public static async Task<IResult> HandleAsync(Guid id, IProductRepository repository, CancellationToken cancellationToken)
    {
        var existing = await repository.GetAssetAsync(id, cancellationToken);
        if (existing is null)
        {
            return Results.NotFound();
        }

        await repository.DeleteAssetAsync(id, cancellationToken);
        return Results.NoContent();
    }
}

public static class GetProductTag
{
    public static async Task<IResult> HandleAsync(Guid productId, Guid tagId, IProductRepository repository, CancellationToken cancellationToken)
    {
        var link = await repository.GetProductTagAsync(productId, tagId, cancellationToken);
        return link is null ? Results.NotFound() : Results.Ok(link);
    }
}

public static class CreateProductTag
{
    public static async Task<IResult> HandleAsync(ProductTag link, IProductRepository repository, CancellationToken cancellationToken)
    {
        var created = await repository.AddProductTagAsync(link, cancellationToken);
        return Results.Created($"{ProductBffAdapter.RoutePrefix}/product-tags/{created.ProductId}/{created.TagId}", created);
    }
}

public static class DeleteProductTag
{
    public static async Task<IResult> HandleAsync(Guid productId, Guid tagId, IProductRepository repository, CancellationToken cancellationToken)
    {
        var existing = await repository.GetProductTagAsync(productId, tagId, cancellationToken);
        if (existing is null)
        {
            return Results.NotFound();
        }

        await repository.DeleteProductTagAsync(productId, tagId, cancellationToken);
        return Results.NoContent();
    }
}

public static class GetProductMarket
{
    public static async Task<IResult> HandleAsync(Guid productId, string marketCode, IProductRepository repository, CancellationToken cancellationToken)
    {
        var link = await repository.GetProductMarketAsync(productId, marketCode, cancellationToken);
        return link is null ? Results.NotFound() : Results.Ok(link);
    }
}

public static class CreateProductMarket
{
    public static async Task<IResult> HandleAsync(ProductMarket link, IProductRepository repository, CancellationToken cancellationToken)
    {
        var created = await repository.AddProductMarketAsync(link, cancellationToken);
        return Results.Created($"{ProductBffAdapter.RoutePrefix}/product-markets/{created.ProductId}/{created.MarketCode}", created);
    }
}

public static class UpdateProductMarket
{
    public static async Task<IResult> HandleAsync(Guid productId, string marketCode, ProductMarket link, IProductRepository repository, CancellationToken cancellationToken)
    {
        var existing = await repository.GetProductMarketAsync(productId, marketCode, cancellationToken);
        if (existing is null)
        {
            return Results.NotFound();
        }

        var updated = link with { ProductId = productId, MarketCode = marketCode };
        await repository.UpdateProductMarketAsync(updated, cancellationToken);
        return Results.Ok(updated);
    }
}

public static class DeleteProductMarket
{
    public static async Task<IResult> HandleAsync(Guid productId, string marketCode, IProductRepository repository, CancellationToken cancellationToken)
    {
        var existing = await repository.GetProductMarketAsync(productId, marketCode, cancellationToken);
        if (existing is null)
        {
            return Results.NotFound();
        }

        await repository.DeleteProductMarketAsync(productId, marketCode, cancellationToken);
        return Results.NoContent();
    }
}

public static class GetAssetMarket
{
    public static async Task<IResult> HandleAsync(Guid assetId, string marketCode, IProductRepository repository, CancellationToken cancellationToken)
    {
        var link = await repository.GetAssetMarketAsync(assetId, marketCode, cancellationToken);
        return link is null ? Results.NotFound() : Results.Ok(link);
    }
}

public static class CreateAssetMarket
{
    public static async Task<IResult> HandleAsync(AssetMarket link, IProductRepository repository, CancellationToken cancellationToken)
    {
        var created = await repository.AddAssetMarketAsync(link, cancellationToken);
        return Results.Created($"{ProductBffAdapter.RoutePrefix}/asset-markets/{created.AssetId}/{created.MarketCode}", created);
    }
}

public static class DeleteAssetMarket
{
    public static async Task<IResult> HandleAsync(Guid assetId, string marketCode, IProductRepository repository, CancellationToken cancellationToken)
    {
        var existing = await repository.GetAssetMarketAsync(assetId, marketCode, cancellationToken);
        if (existing is null)
        {
            return Results.NotFound();
        }

        await repository.DeleteAssetMarketAsync(assetId, marketCode, cancellationToken);
        return Results.NoContent();
    }
}
