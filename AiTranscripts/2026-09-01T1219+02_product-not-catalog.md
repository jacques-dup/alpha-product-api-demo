# Session 2026-09-01T1219+02

**Session timestamp:** 2026-09-01T12:19+02:00

## User input

Add to cursor rules to stop using the term "Catalog" for this. Instead we should be using "Product"

## Assistant output

Added always-apply Cursor rule `.cursor/rules/product-naming.mdc`: call this system **Product**, not Catalog (paths, tags, types, new prose). Aligned sibling rules. Renamed WebApi prefix `/catalog` ? `/product` and types `CatalogSnapshot` / `ProductCatalogMapper` to `ProductSnapshot` / `ProductPayloadMapper`.
