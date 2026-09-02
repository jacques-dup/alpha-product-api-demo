# CLAUDE.md

This file is the **Claude Code / Rider** copy of `.cursor/rules/`. Keep both in sync (see `.cursor/rules/claude-md-sync.mdc`).

This repo is a Head of Engineering technical exercise. Use AI for documentation, plans, diagrams, a minimal writeup, activity/edit tracking, research, and code.

## Directories

- `Brief/` — original brief PDF plus a markdown summary for ingestion. Do not alter the PDF.
- `Documentation/` — architectural, planning, and reasoning docs as `.md` files.
- `Solution/` — all product/application code.
- `AiTranscripts/` — every AI interaction as a timestamped `.md` file (including the current session).
- `TimeTracking/` — engagement tracking. Canonical file: `TimeTracking/timeline.md`.
- `CLAUDE.md` — this file. Claude Code / Rider copy of `.cursor/rules/`.
- `.cursor/rules/` — Cursor copy of the same conventions (YAML frontmatter on each `.mdc`).

## AI transcripts (`AiTranscripts/`)

After each session, write or update a transcript `.md` named `YYYY-MM-DDTHHMM+TZ_short-slug.md`.

Include:

- Session timestamp (ISO-like, with timezone)
- Exact user input
- Exact model/user-facing output

Do **not** include analyzing thoughts, internal reasoning, tool-call traces, or hidden chain-of-thought.

## Time tracking (`TimeTracking/timeline.md`)

Automatically **append** a session entry when work happens in this project: timestamp, short title, and what was done.

- **Additions only.** Never delete, rewrite, or reorder existing entries.
- The candidate may add manual entries for work done outside AI (including Rider); leave those untouched.

## Docs and code

- Put plans, architecture, diagrams (as markdown/mermaid), and reasoning in `Documentation/`.
- Put implementation in `Solution/`.
- Keep writeups minimal unless asked otherwise.

Canonical product doc: `Documentation/product-and-technical-dossier.md`. Baseline scope is §1.7.

## Portal packages (pnpm)

- Package manager for `Solution/product-portal` is **pnpm**, never npm or yarn (`pnpm-lock.yaml` only).
- Allowed without asking: **Vite**, **React**, **TypeScript**, **React Admin**, **Vitest** (and the official Vite `react-ts` template's own tooling).
- **Do not** add any other package, plugin, or UI kit until the user **explicitly approves** it in chat. If a library would help (for example `ra-data-simple-rest`, jsdom, MUI extras), **ask first** and wait.
- Transitive dependencies of an approved package are fine; do not add extra direct dependencies around them.

---

## Product (not Catalog)

This exercise is **Product** (`Product.WebApi`, `Product.Bff`, `Product.Domain`, `Product.ProductStore`, `Product.ApplicationRoot`). Do **not** name the product, API, module, route prefix, OpenAPI tag, or types **Catalog**.

Use **Product** in docs, transcripts (assistant output), timeline entries, code comments, READMEs, route groups, and identifiers.

```text
? /catalog, WithTags("Catalog"), CatalogSnapshot, ProductCatalogMapper, "catalog API"
? /product, WithTags("Product"), ProductSnapshot, ProductPayloadMapper, Product.WebApi
```

Do not rename SQL tables or columns (`product`, `product_family`, …). Prefer “product data” / “product payload” over “catalog” in new prose. Do not rewrite locked dossier history unless the user asks; new writing follows this rule.

---

## Titles for reference systems

Use these titles in docs, transcripts (assistant output), timeline entries, code comments, and READMEs. **Never modify** those other repositories. Keep design and code inside this exercise (`Documentation/`, `Solution/`).

**Local clone paths are AI-only.** Use them only to inspect those systems. **Never** write those filesystem paths in `Documentation/`, `Brief/`, `TimeTracking/`, `Solution/`, or other submitted docs.

### Core Services (Profile)

Call it **Core Services** (the **Profile** module).

- Local (AI only): `/Users/jdples/Repositories/CoreServices` — Profile is nested (`Profile/src/...` plus `CoreServices.ApplicationRoot`). Inspect **ApplicationRoot** composition only (entrypoint plugs adapters).
- Inspiration only: **ApplicationRoot** is the process entrypoint; Web API (and here, BFF) are **adapters registered as services**. This exercise is a single service, not a modular monolith.
- Do **not** copy: double folder nesting, reverse-proxy to inner Kestrel hosts, `Driven.*` / `Driver.*` project names, Service Bus, cron, or extra background adapters.

### Alpha Identity Provider

Call it **IDP** or **Alpha Identity Provider** (self-hosted Duende IdentityService).

- Local (AI only): `/Users/jdples/Repositories/IdentityService`.
- **Do not change the IDP service:** policies, users, themes, roles, claims, grants, extra scopes, or server configuration. Demo OAuth clients for this POC may be registered; client ids/secrets go in **env files after the project is scaffolded**, not in the IDP codebase.
- IDP has scaffolding for role-based access, but **no roles are assigned** and **role claims are not returned**. Do not design this product as if role claims exist. Do not add roles in IDP to make the POC work.
- Consume IDP as standard OAuth/OIDC (dev discovery only). Use existing scopes **`alpha.idp.read`** (WebApi, client credentials) and **`alpha.idp.readwrite`** (portal BFF, interactive). Do not add scopes or roles in IDP.
- Portal authorization beyond scopes is an **allow-list stub in this product** (configured IDP account ids), not IDP role claims. Do not store user profiles in this service.

### MyAlpha BFF

Call it **MyAlpha BFF**.

- Local (AI only): `/Users/jdples/Repositories/MyAlpha/myalpha-bff`. Inspect structure only (Duende BFF host, `Endpoints/Api`, `Driven.*` adapters, `SharedKernel` ports).
- Shape reference for this product's C# modularisation (inbound host + driven persistence). Do not rebuild MyAlpha, copy WordPress/MySQL access, Service Bus, HMAC clients, token-exchange to other Alpha APIs, or YARP remote-API proxying.

### Whatsapp Content Distribution

Call it **Whatsapp Content Distribution**.

- Local (AI only): `/Users/jdples/Repositories/whatsapp/whatsapp-content`.
- Static site: decodes a `?d=` payload, looks up `courseType:audience:country:language`, and renders a variant from a static JSON manifest. It **motivated** this product (hard-coded JSON, no portal). This exercise is **not** a drop-in replacement for that manifest and is **not** measured against that site. Do not integrate it. If this product later looks viable, a consuming service can be adapted then.
- Do not rebuild Whatsapp Content Distribution, the WhatsApp registration worker, MyAlpha, the Guest App, or a full Courses platform.

---

## Product database (local)

Applies when working on `Solution/Data/`, `Product.ProductStore`, `Product.Domain`, or `Documentation/product-store-build-plan.md`.

- Database name: **`product_service`** (local Postgres). Schema source of truth: `Solution/Data/product_schema.sql`. Validate with `Solution/Data/product_schema_smoketest.sql`.
- After changing the SQL file: re-apply it to `product_service`, then run the smoke test.
- Country–market ACL is **not** a table. Audience is many-to-many (no unique-audience constraint).
- **Product.ProductStore** implements `IProductRepository` via **ProductRepository**. Product rules (language fallback, country ACL, API contract) stay in Domain.
- This project has a [DBHub](https://dbhub.ai/) MCP server (`dbhub`) pointed at `product_service`. Prefer `search_objects` then `execute_sql` when inspecting or adjusting product data. Do not invent schema in the database that is not also in `product_schema.sql`.
