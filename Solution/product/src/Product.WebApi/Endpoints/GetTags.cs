using Product.Domain;

namespace Product.WebApi.Endpoints;

public static class GetTags
{
    public static async Task<IResult> HandleAsync(
        IProductRepository repository,
        CancellationToken cancellationToken)
    {
        var tags = await repository.ListTagsAsync(cancellationToken);
        return Results.Ok(tags);
    }
}
