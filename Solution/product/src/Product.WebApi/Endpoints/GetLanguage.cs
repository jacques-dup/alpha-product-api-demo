using Product.Domain;

namespace Product.WebApi.Endpoints;

public static class GetLanguage
{
    public static async Task<IResult> HandleAsync(
        string code,
        IProductRepository repository,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return Results.Problem(statusCode: StatusCodes.Status400BadRequest, detail: "Language code is required.");
        }

        var language = await repository.GetLanguageAsync(code, cancellationToken);
        return language is null ? Results.NotFound() : Results.Ok(language);
    }
}
