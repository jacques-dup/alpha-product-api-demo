using Product.Domain;

namespace Product.WebApi.Endpoints;

public static class GetFamily
{
    public static async Task<IResult> HandleAsync(
        Guid id,
        IProductRepository repository,
        CancellationToken cancellationToken)
    {
        var family = await repository.GetProductFamilyAsync(id, cancellationToken);
        return family is null ? Results.NotFound() : Results.Ok(family);
    }
}
