using Product.Domain;

namespace Product.WebApi.Endpoints;

public static class GetMarkets
{
    public static async Task<IResult> HandleAsync(
        IProductRepository repository,
        CancellationToken cancellationToken)
    {
        var markets = await repository.ListMarketsAsync(cancellationToken);
        return Results.Ok(markets);
    }
}
