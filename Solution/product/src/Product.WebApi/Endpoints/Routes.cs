using Microsoft.AspNetCore.Http.Metadata;
using Product.Domain;
using Product.WebApi.Authentication;

namespace Product.WebApi.Endpoints;

public static class Routes
{
    private const string ApiTag = "Product";

    public static RouteGroupBuilder MapApi(this RouteGroupBuilder group)
    {
        // Applied to the group, not per route, so a new route cannot be added unauthenticated
        // by omission. Product data is never served anonymously (FR-API-05).
        group.RequireAuthorization(ProductApiAuthentication.ReadPolicy)
            .WithMetadata(
                new ProducesResponseTypeMetadata(StatusCodes.Status401Unauthorized),
                new ProducesResponseTypeMetadata(StatusCodes.Status403Forbidden));

        group.MapGet("languages", GetLanguages.HandleAsync)
            .Produces<List<Language>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithTags(ApiTag);

        group.MapGet("languages/{code}", GetLanguage.HandleAsync)
            .Produces<Language>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithTags(ApiTag);

        group.MapGet("markets", GetMarkets.HandleAsync)
            .Produces<List<Market>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithTags(ApiTag);

        group.MapGet("markets/{code}", GetMarket.HandleAsync)
            .Produces<Market>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithTags(ApiTag);

        group.MapGet("families", GetFamilies.HandleAsync)
            .Produces<List<ProductFamily>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithTags(ApiTag);

        group.MapGet("families/{id:guid}", GetFamily.HandleAsync)
            .Produces<ProductFamily>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithTags(ApiTag);

        group.MapGet("tags", GetTags.HandleAsync)
            .Produces<List<Tag>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithTags(ApiTag);

        group.MapGet("tags/{id:guid}", GetTag.HandleAsync)
            .Produces<Tag>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithTags(ApiTag);

        group.MapGet("products", GetProducts.HandleAsync)
            .Produces<List<ProductPayload>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithTags(ApiTag);

        group.MapGet("products/code/{code}", GetProductByCode.HandleAsync)
            .Produces<ProductPayload>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithTags(ApiTag);

        group.MapGet("products/{id:guid}", GetProduct.HandleAsync)
            .Produces<ProductPayload>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithTags(ApiTag);

        group.MapGet("items", GetProductItems.HandleAsync)
            .Produces<List<ProductItem>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithTags(ApiTag);

        group.MapGet("items/{id:guid}", GetProductItem.HandleAsync)
            .Produces<ProductItem>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithTags(ApiTag);

        group.MapGet("assets", GetAssets.HandleAsync)
            .Produces<List<Asset>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithTags(ApiTag);

        group.MapGet("assets/{id:guid}", GetAsset.HandleAsync)
            .Produces<Asset>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithTags(ApiTag);

        group.MapGet("product-tags", GetProductTags.HandleAsync)
            .Produces<List<ProductTag>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithTags(ApiTag);

        group.MapGet("product-markets", GetProductMarkets.HandleAsync)
            .Produces<List<ProductMarket>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithTags(ApiTag);

        group.MapGet("asset-markets", GetAssetMarkets.HandleAsync)
            .Produces<List<AssetMarket>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithTags(ApiTag);

        return group;
    }
}
