using Product.Domain;

namespace Product.WebApi.Endpoints;

public static class GetLanguages
{
    public static async Task<IResult> HandleAsync(
        IProductRepository repository,
        CancellationToken cancellationToken)
    {
        var languages = await repository.ListLanguagesAsync(cancellationToken);
        return Results.Ok(languages);
    }
}
