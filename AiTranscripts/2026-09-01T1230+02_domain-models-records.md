# Session 2026-09-01T1230+02

**Session timestamp:** 2026-09-01T12:30+02:00

## User input

I'd prefer using sealed records for the dtos and models where possible since they only represent data objects. Also move them into a Models/ directory to clean up the file structure.

## Assistant output

Moved catalog models to `Product.Domain/Models/` as sealed records with `init` accessors. New ids use `with`. WebApi `CatalogSnapshot` is a sealed record; API payloads were already records. Namespace stays `Product.Domain`. `dotnet test` passes.
