# Product.ProductStore build plan

Implement the Postgres **repository** (`Product.ProductStore`) against the catalog already defined in dossier section 2.7 and `Solution/Data/product_schema.sql`. This is persistence only. Catalog rules stay in **Product.Domain**.

**Local database (this machine):** `product_service` on localhost:5432. Schema applied; smoke test passed. [DBHub](https://dbhub.ai/) MCP is configured for this project so prompts can inspect and change data against that database.

## Boundaries (from the dossier)

| In Product.ProductStore | Not in Product.ProductStore |
| --- | --- |
| Load and save rows for the 10 catalog tables | Country to market ACL (Domain, coded when we need it) |
| Transactions for a product graph (items, assets, tags, markets) | Language fallback to `product.content_language` (Domain) |
| Map SQL constraints to clear failures | Assembling the baseline API contract (Domain / WebApi) |
| Register with `AddProductStore` on ApplicationRoot | Auth, BFF, portal, seed of production media |

SQL file is the schema source of truth. Do **not** add EF migrations that diverge from `product_schema.sql`. After any schema change: update the SQL file, re-apply to `product_service`, run `product_schema_smoketest.sql`.

## Port vs adapter

```text
Product.Domain               entities + IProductRepository
Product.ProductStore         ProductStoreAdapter: scoped ProductDbContext + ProductRepository
Product.ApplicationRoot      AddProductModules -> AddProductStore first
```

No `Product.Application` project. Bff and WebApi call the port in-process.

## `IProductRepository` / `ProductRepository`

Keep the port on **aggregates and lookups**, not one method per column.

**Lookups** (portal CRUD, seed): `Language`, `Market`, `ProductFamily`, `Tag` ù list, get, save, delete.

**Product graph** (the catalogue unit):

- `GetProduct(Guid id)` / `GetProductByCode(string code)` ù product plus items, assets, tags, markets
- `ListProducts(...)` ù list rows (family, tags, markets as needed for the portal list); full graph on get
- `SaveProduct(Product graph)` ù upsert in one transaction (product, items, assets, `product_tag`, `product_market`, `asset_market`)
- `DeleteProduct(Guid id)` ù rely on `ON DELETE CASCADE` for children; family delete stays `RESTRICT`

Do **not** add a unique-one-audience method. Tags are many-to-many; the API filters later.

List filters that are just SQL (`family_id`, `market_code`, tag id) can live on the store. Caller-country ACL and ùlanguage omittedù stay out of SQL until Domain has a place to put them.

## Entities

Mirror section 2.7 / the SQL file. Closed enums as strings matching CHECKs (`episode`/`training`, `country`/`region`, asset `role`/`kind`/`provider`). `Asset.ItemId` nullable. No `default_audience` column. No country-ACL table.

## Implementation notes

- EF Core 8 + **Npgsql**. `ProductDbContext` maps the SQL tables. No EF migrations; `product_schema.sql` remains source of truth.
- `ProductRepository` is table-level CRUD (list/get/add/update/delete) for all 10 tables.
- Map unique/FK/CHECK failures to small Domain or store exceptions the BFF can turn into 409/400 later ù do not leak Postgres messages to clients.
- Composite rule already in SQL: item-level assets must share `product_id` with the item. Store should set both columns; do not fight the FK.

## Tests (`Product.ProductStore.Tests`)

Dossier: unit + store acceptance against Postgres. Use `product_service` (same DSN as ApplicationRoot).

Priority cases:

1. Save product graph, get by id/code, round-trip items and language-specific assets
2. Two audience tags on one product
3. Product-level asset (`item_id` null) and item-level asset
4. Unique `product.code` / `(product_id, code)` on items
5. Delete product cascades children; cannot delete family that still has products

Do not require Testcontainers if local `product_service` is the agreed fixture. Isolate tests with distinct codes/ids; do not drop the public schema.

## Wiring

`AddProductStore` (`ProductStoreAdapter`) reads `ConnectionStrings:ProductStore` (env `ConnectionStrings__ProductStore`). Fail fast if missing. Registers **scoped** `ProductDbContext`, **scoped** options, and **scoped** `IProductRepository`. ApplicationRoot `AddProductModules` calls it before Bff and WebApi so those modules can inject the port.

Local DSN (trust, no password): `postgres://jdples@localhost:5432/product_service`

## Build order

| Step | What | Done when |
| --- | --- | --- |
| 0 | Database `product_service` + schema + DBHub MCP | Done (this session) |
| 1 | Domain entities + `IProductRepository` + `ProductDbContext` / `ProductRepository` | Done |
| 2 | Lookups: language, market, family, tag | Store tests pass for those tables |
| 3 | `SaveProduct` / `GetProduct` graph | Round-trip test with two languages on one episode |
| 4 | List + delete + constraint mapping | Store tests for uniques and cascade |
| 5 | Point ApplicationRoot connection string at `product_service` | Host starts; no catalog HTTP yet |

Seed data and WebApi/BFF come **after** the store can persist a graph.

## DBHub while building

Project MCP: `.cursor/mcp.json` (stdio, [DBHub](https://dbhub.ai/) ? `product_service`). Use `search_objects` then `execute_sql`. Schema edits still land in `Solution/Data/product_schema.sql`, then re-apply and smoke test. Do not treat ad-hoc SQL in the database as the source of truth.
