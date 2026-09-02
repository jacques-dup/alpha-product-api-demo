# Product

A Head of Engineering technical exercise: a **Product** service that staff can edit in a portal and that other services can read over HTTP. One C# process (**Product.ApplicationRoot**) hosts a cookie BFF (`/api`, OIDC to the Alpha Identity Provider, allow-list stub) and a read-only Web API (`/product`, client-credentials bearer). Product rules live in **Product.Domain**; Postgres persistence is **Product.ProductStore**. The staff UI is a React Admin SPA (`product-portal`) served from the same origin. The live Azure host is a single App Service plus a public Flexible Server, with the built SPA merged into `wwwroot` so `/` is the portal.

**Product and technical dossier:** [Documentation/product-and-technical-dossier.md](Documentation/product-and-technical-dossier.md)

## Directories

| Path | What it holds |
| --- | --- |
| [Brief/](Brief/) | Original exercise PDF and a markdown summary for ingestion. The PDF is not edited. |
| [Documentation/](Documentation/) | The dossier (product, design, quality, and �4 future/production) plus a short ProductStore build plan. |
| [Solution/product/](Solution/product/) | C# solution (`Product.sln`): ApplicationRoot, Bff, WebApi, Domain, ProductStore, and tests. |
| [Solution/product-portal/](Solution/product-portal/) | Portal SPA (Vite, React, TypeScript, React Admin, pnpm). Locally `pnpm dev`; in Azure the production `dist/` is copied into ApplicationRoot `wwwroot`. |
| [Solution/Data/](Solution/Data/) | Schema (`product_schema.sql`), smoke test, and example seed. Apply to local `product_service` or the Azure Flexible Server. |
| [AiTranscripts/](AiTranscripts/) | Timestamped records of AI-assisted sessions. |
| [TimeTracking/](TimeTracking/) | Engagement log. Canonical file: [TimeTracking/timeline.md](TimeTracking/timeline.md). |
| [CLAUDE.md](CLAUDE.md) | Exercise conventions (also in `.cursor/rules/` for Cursor). |
