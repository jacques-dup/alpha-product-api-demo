# Session 2026-09-01T1225+02

**Session timestamp:** 2026-09-01T12:25+02:00

## User input

this is a new agent that will look at the product dossier and implement the baseline webAPI adapter for the project. The main WebApi adapter will orchestrate the module. All endpoint handlers can be created inside an Endpoints/ direcory in the webapi adapter project. also in the Endpoint directory will be a Routes.cs file that does all the route mapping like:

public static RouteGroupBuilder MapApi(this RouteGroupBuilder group)
    {
        group.MapGet("some/endpoint", GetSomeEndpoint.HandleAsync)
            .Produces<List<AlphaCourseSummary>>()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization()
            .AsBffApiEndpoint()
            .WithTags(ApiTag);
}

Using the OpenApi chaining so that our eventual api documentation is easier to generate.
Focus on all the get routes first and ignore the authentication step for now - we'll add that later when the clients are configured. I first want to test that the API has a working base.

## Assistant output

Product.WebApi now maps unauthenticated GET routes under `/catalog`. Handlers live in `Endpoints/` with `Routes.MapApi` using `.Produces` / `.ProducesProblem` / `.WithTags` (no `RequireAuthorization` or `AsBffApiEndpoint` yet). Product list/get returns the baseline payload (family, tags, markets, items, language-selected assets). Query params: `courseType`, `audience`, `country`, `language`. Country maps through Domain `CountryMarketAcl`; omitted language falls back to product `content_language`. `dotnet test` on Product.sln passed.
