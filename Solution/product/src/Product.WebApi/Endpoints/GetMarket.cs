using Product.Domain;

namespace Product.WebApi.Endpoints;

public static class GetMarket
{
    public static async Task<IResult> HandleAsync(
        string code,
        IProductRepository repository,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return Results.Problem(statusCode: StatusCodes.Status400BadRequest, detail: "Market code is required.");
        }

        var market = await repository.GetMarketAsync(code, cancellationToken);
        return market is null ? Results.NotFound() : Results.Ok(market);
    }
}
