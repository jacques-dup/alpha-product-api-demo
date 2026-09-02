using Duende.Bff;
using Microsoft.AspNetCore.Http.Metadata;
using Product.Bff.Authentication;
using Product.Domain;
using Product.WebApi.Endpoints;

namespace Product.Bff.Endpoints;

public static class Routes
{
    private const string ApiTag = "Portal";

    public static RouteGroupBuilder MapApi(this RouteGroupBuilder group)
    {
        group.RequireAuthorization(ProductBffAuthentication.AccessPolicy)
            .AsBffApiEndpoint()
            .WithMetadata(
                new ProducesResponseTypeMetadata(StatusCodes.Status401Unauthorized),
                new ProducesResponseTypeMetadata(StatusCodes.Status403Forbidden));

        MapReads(group);
        MapWrites(group);
        return group;
    }

    private static void MapReads(RouteGroupBuilder group)
    {
        group.MapGet("languages", ListLanguages.HandleAsync)
            .Produces<List<Language>>()
            .WithTags(ApiTag);

        group.MapGet("languages/{code}", GetLanguageByCode.HandleAsync)
            .Produces<Language>()
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(ApiTag);

        group.MapGet("markets", ListMarkets.HandleAsync)
            .Produces<List<Market>>()
            .WithTags(ApiTag);

        group.MapGet("markets/{code}", GetMarketByCode.HandleAsync)
            .Produces<Market>()
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(ApiTag);

        group.MapGet("families", ListFamilies.HandleAsync)
            .Produces<List<ProductFamily>>()
            .WithTags(ApiTag);

        group.MapGet("families/{id:guid}", GetFamilyById.HandleAsync)
            .Produces<ProductFamily>()
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(ApiTag);

        group.MapGet("tags", ListTags.HandleAsync)
            .Produces<List<Tag>>()
            .WithTags(ApiTag);

        group.MapGet("tags/{id:guid}", GetTagById.HandleAsync)
            .Produces<Tag>()
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(ApiTag);

        group.MapGet("products", ListProducts.HandleAsync)
            .Produces<List<ProductPayload>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithTags(ApiTag);

        group.MapGet("products/code/{code}", GetProductByCode.HandleAsync)
            .Produces<ProductPayload>()
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(ApiTag);

        group.MapGet("products/{id:guid}", GetProductById.HandleAsync)
            .Produces<ProductPayload>()
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(ApiTag);

        group.MapGet("items", ListItems.HandleAsync)
            .Produces<List<ProductItem>>()
            .WithTags(ApiTag);

        group.MapGet("items/{id:guid}", GetItemById.HandleAsync)
            .Produces<ProductItem>()
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(ApiTag);

        group.MapGet("assets", ListAssets.HandleAsync)
            .Produces<List<Asset>>()
            .WithTags(ApiTag);

        group.MapGet("assets/{id:guid}", GetAssetById.HandleAsync)
            .Produces<Asset>()
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(ApiTag);

        group.MapGet("product-tags", ListProductTags.HandleAsync)
            .Produces<List<ProductTag>>()
            .WithTags(ApiTag);

        group.MapGet("product-tags/{productId:guid}/{tagId:guid}", GetProductTag.HandleAsync)
            .Produces<ProductTag>()
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(ApiTag);

        group.MapGet("product-markets", ListProductMarkets.HandleAsync)
            .Produces<List<ProductMarket>>()
            .WithTags(ApiTag);

        group.MapGet("product-markets/{productId:guid}/{marketCode}", GetProductMarket.HandleAsync)
            .Produces<ProductMarket>()
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(ApiTag);

        group.MapGet("asset-markets", ListAssetMarkets.HandleAsync)
            .Produces<List<AssetMarket>>()
            .WithTags(ApiTag);

        group.MapGet("asset-markets/{assetId:guid}/{marketCode}", GetAssetMarket.HandleAsync)
            .Produces<AssetMarket>()
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(ApiTag);
    }

    private static void MapWrites(RouteGroupBuilder group)
    {
        group.MapPost("languages", CreateLanguage.HandleAsync)
            .Produces<Language>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithTags(ApiTag);

        group.MapPut("languages/{code}", UpdateLanguage.HandleAsync)
            .Produces<Language>()
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(ApiTag);

        group.MapDelete("languages/{code}", DeleteLanguage.HandleAsync)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(ApiTag);

        group.MapPost("markets", CreateMarket.HandleAsync)
            .Produces<Market>(StatusCodes.Status201Created)
            .WithTags(ApiTag);

        group.MapPut("markets/{code}", UpdateMarket.HandleAsync)
            .Produces<Market>()
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(ApiTag);

        group.MapDelete("markets/{code}", DeleteMarket.HandleAsync)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(ApiTag);

        group.MapPost("families", CreateFamily.HandleAsync)
            .Produces<ProductFamily>(StatusCodes.Status201Created)
            .WithTags(ApiTag);

        group.MapPut("families/{id:guid}", UpdateFamily.HandleAsync)
            .Produces<ProductFamily>()
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(ApiTag);

        group.MapDelete("families/{id:guid}", DeleteFamily.HandleAsync)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(ApiTag);

        group.MapPost("tags", CreateTag.HandleAsync)
            .Produces<Tag>(StatusCodes.Status201Created)
            .WithTags(ApiTag);

        group.MapPut("tags/{id:guid}", UpdateTag.HandleAsync)
            .Produces<Tag>()
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(ApiTag);

        group.MapDelete("tags/{id:guid}", DeleteTag.HandleAsync)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(ApiTag);

        group.MapPost("products", CreateProduct.HandleAsync)
            .Produces<Domain.Product>(StatusCodes.Status201Created)
            .WithTags(ApiTag);

        group.MapPut("products/{id:guid}", UpdateProduct.HandleAsync)
            .Produces<Domain.Product>()
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(ApiTag);

        group.MapDelete("products/{id:guid}", DeleteProduct.HandleAsync)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(ApiTag);

        group.MapPost("items", CreateItem.HandleAsync)
            .Produces<ProductItem>(StatusCodes.Status201Created)
            .WithTags(ApiTag);

        group.MapPut("items/{id:guid}", UpdateItem.HandleAsync)
            .Produces<ProductItem>()
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(ApiTag);

        group.MapDelete("items/{id:guid}", DeleteItem.HandleAsync)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(ApiTag);

        group.MapPost("assets", CreateAsset.HandleAsync)
            .Produces<Asset>(StatusCodes.Status201Created)
            .WithTags(ApiTag);

        group.MapPut("assets/{id:guid}", UpdateAsset.HandleAsync)
            .Produces<Asset>()
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(ApiTag);

        group.MapDelete("assets/{id:guid}", DeleteAsset.HandleAsync)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(ApiTag);

        group.MapPost("product-tags", CreateProductTag.HandleAsync)
            .Produces<ProductTag>(StatusCodes.Status201Created)
            .WithTags(ApiTag);

        group.MapDelete("product-tags/{productId:guid}/{tagId:guid}", DeleteProductTag.HandleAsync)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(ApiTag);

        group.MapPost("product-markets", CreateProductMarket.HandleAsync)
            .Produces<ProductMarket>(StatusCodes.Status201Created)
            .WithTags(ApiTag);

        group.MapPut("product-markets/{productId:guid}/{marketCode}", UpdateProductMarket.HandleAsync)
            .Produces<ProductMarket>()
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(ApiTag);

        group.MapDelete("product-markets/{productId:guid}/{marketCode}", DeleteProductMarket.HandleAsync)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(ApiTag);

        group.MapPost("asset-markets", CreateAssetMarket.HandleAsync)
            .Produces<AssetMarket>(StatusCodes.Status201Created)
            .WithTags(ApiTag);

        group.MapDelete("asset-markets/{assetId:guid}/{marketCode}", DeleteAssetMarket.HandleAsync)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(ApiTag);
    }
}
