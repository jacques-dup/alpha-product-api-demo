using Product.Domain;

namespace Product.WebApi.Endpoints;

public static class GetFamilies
{
    public static async Task<IResult> HandleAsync(
        IProductRepository repository,
        CancellationToken cancellationToken)
    {
        var families = await repository.ListProductFamiliesAsync(cancellationToken);
        return Results.Ok(families);
    }
}
