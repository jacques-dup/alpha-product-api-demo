# Product

A Head of Engineering technical exercise: a **Product** service that staff can edit in a portal and that other services can read over HTTP. One C# process (**Product.ApplicationRoot**) hosts a cookie BFF (`/api`, OIDC to the Alpha Identity Provider, allow-list stub) and a read-only Web API (`/product`, client-credentials bearer). Product rules live in **Product.Domain**; Postgres persistence is **Product.ProductStore**. The staff UI is a React Admin SPA (`product-portal`) served from the same origin. The live Azure host is a single App Service plus a public Flexible Server, with the built SPA merged into `wwwroot` so `/` is the portal.

**Product and technical dossier:** [Documentation/product-and-technical-dossier.md](Documentation/product-and-technical-dossier.md)

## Run locally

Needs **.NET 8**, **PostgreSQL 13+**, **Node.js** with **pnpm** 9, and a demo OAuth client on the Alpha Identity Provider (`https://dev.auth.alpha.org`).

### 1. Database

Create a local database named `product_service`, then apply the schema (and optionally the example seed):

```bash
psql -d product_service -f Solution/Data/product_schema.sql
psql -d product_service -f Solution/Data/seed_product_data_example.sql
```

Point `ConnectionStrings:ProductStore` at that database (user-secrets below, or `appsettings.json`). Username and password must match your local Postgres.

### 2. Secrets

From `Solution/product/src/Product.ApplicationRoot`:

```bash
dotnet user-secrets set "Bff:ClientId" "your-bff-client-id"
dotnet user-secrets set "Bff:ClientSecret" "your-bff-client-secret"
dotnet user-secrets set "WebApi:ClientId" "your-webapi-client-id"
dotnet user-secrets set "WebApi:ClientSecret" "your-webapi-client-secret"
dotnet user-secrets set "Portal:AllowList:0" "idp-subject-or-email"
```

The BFF client must allow `https://localhost:5173/signin-oidc` and `https://localhost:7127/signin-oidc` (and matching `/signout-callback-oidc` post-logout URIs). Empty `Bff:ClientId` skips OIDC so tests can start; the portal login will not work until it is set.

### 3. API

```bash
cd Solution/product
dotnet run --launch-profile https --project src/Product.ApplicationRoot
```

HTTPS is `https://localhost:7127` (Swagger at `/swagger`). Web API reads are `/product` with a client-credentials bearer (`alpha.idp.read`). Portal CRUD is `/api` behind the BFF cookie.

### 4. Portal

In another terminal:

```bash
cd Solution/product-portal
dotnet dev-certs https --export-path ./certs/localhost.pem --format Pem --no-password
pnpm install
pnpm dev
```

Open `https://localhost:5173/`. Vite proxies `/api`, `/bff`, and the OIDC callbacks to ApplicationRoot. Sign-in is `/bff/login` against the IDP; after login the portal opens `/#/products` if your account is on the allow-list.

### Tests

```bash
dotnet test Solution/product/Product.sln
cd Solution/product-portal && pnpm test
```

More portal detail: [Solution/product-portal/README.md](Solution/product-portal/README.md).

## Directories

| Path | What it holds |
| --- | --- |
| [Brief/](Brief/) | Original exercise PDF and a markdown summary for ingestion. The PDF is not edited. |
| [Documentation/](Documentation/) | The dossier (product, design, quality, and §4 future/production) plus a short ProductStore build plan. |
| [Solution/product/](Solution/product/) | C# solution (`Product.sln`): ApplicationRoot, Bff, WebApi, Domain, ProductStore, and tests. |
| [Solution/product-portal/](Solution/product-portal/) | Portal SPA (Vite, React, TypeScript, React Admin, pnpm). Locally `pnpm dev`; in Azure the production `dist/` is copied into ApplicationRoot `wwwroot`. |
| [Solution/Data/](Solution/Data/) | Schema (`product_schema.sql`), smoke test, and example seed. Apply to local `product_service` or the Azure Flexible Server. |
| [AiTranscripts/](AiTranscripts/) | Timestamped records of AI-assisted sessions. |
| [TimeTracking/](TimeTracking/) | Engagement log. Canonical file: [TimeTracking/timeline.md](TimeTracking/timeline.md). |
| [CLAUDE.md](CLAUDE.md) | Exercise conventions (also in `.cursor/rules/` for Cursor). |
