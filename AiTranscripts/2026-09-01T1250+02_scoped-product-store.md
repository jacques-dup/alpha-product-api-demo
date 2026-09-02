# Session 2026-09-01T1250+02

**Session timestamp:** 2026-09-01T12:50+02:00

## User input

register the repository and associated services in ApplicationRoot so that it can be used by the solution's modules. The database services should be scoped services as opposed to singleton

## Assistant output

ApplicationRoot now plugs the store **before** Bff and WebApi, and every database service is **scoped** (not singleton).

`ProductStoreAdapter` registers scoped `ProductDbContext`, scoped `DbContextOptions`, and scoped `IProductRepository` / `ProductRepository`. EF Core’s default is scoped context with **singleton options**; both lifetimes are now scoped so each request gets its own unit of work.

`Program` calls `AddProductModules`, which runs `AddProductStore` first so Product.WebApi (and later Product.Bff) can inject `IProductRepository` in-process.
