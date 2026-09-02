using Product.Domain;

namespace Product.WebApi.Endpoints;

public static class GetProductMarkets
{
    public static async Task<IResult> HandleAsync(
        IProductRepository repository,
        Guid? productId,
        CancellationToken cancellationToken)
    {
        var links = await repository.ListProductMarketsAsync(cancellationToken);
        if (productId is { } id)
        {
            links = links.Where(link => link.ProductId == id).ToList();
        }

        return Results.Ok(links);
    }
}
