using Product.Domain;

namespace Product.WebApi.Endpoints;

public static class GetProductItem
{
    public static async Task<IResult> HandleAsync(
        Guid id,
        IProductRepository repository,
        CancellationToken cancellationToken)
    {
        var item = await repository.GetProductItemAsync(id, cancellationToken);
        return item is null ? Results.NotFound() : Results.Ok(item);
    }
}
