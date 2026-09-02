using Product.Domain;

namespace Product.WebApi.Endpoints;

public static class GetProductItems
{
    public static async Task<IResult> HandleAsync(
        IProductRepository repository,
        Guid? productId,
        CancellationToken cancellationToken)
    {
        var items = await repository.ListProductItemsAsync(cancellationToken);
        if (productId is { } id)
        {
            items = items.Where(item => item.ProductId == id).ToList();
        }

        return Results.Ok(items);
    }
}
