using Product.Domain;

namespace Product.WebApi.Endpoints;

public static class GetTag
{
    public static async Task<IResult> HandleAsync(
        Guid id,
        IProductRepository repository,
        CancellationToken cancellationToken)
    {
        var tag = await repository.GetTagAsync(id, cancellationToken);
        return tag is null ? Results.NotFound() : Results.Ok(tag);
    }
}
