# Timeline

Additions only. Do not remove, rewrite, or reorder existing entries.

## 2026-08-31 11:31 (UTC+2) — Session: record project conventions

- Recorded AI usage and repo layout conventions for this technical exercise.
- Created `AiTranscripts/`, `Documentation/`, `Solution/`, and `TimeTracking/` (Brief already present).
- Added `Brief/brief-summary.md` from the original PDF.
- Added always-apply Cursor rule `.cursor/rules/technical-exercise-conventions.mdc`.
- Recorded this session in `AiTranscripts/2026-08-31T1131+02_record-project-conventions.md`.

## 2026-08-31 11:34 (UTC+2) — Session: brief summary doc

- Updated `Brief/brief-summary.md` with an at-a-glance table plus a full structured restatement of the PDF.
- Recorded this session in `AiTranscripts/2026-08-31T1134+02_brief-summary.md`.

## 2026-08-31 11:37 (UTC+2) — Session ended: pause before solution choice

- Ended this setup session.
- Next: candidate will return after deciding what solution to build.
- Recorded this session in `AiTranscripts/2026-08-31T1137+02_session-end.md`.

## 2026-08-31 20:01 (UTC+2) — Session started: product and technical dossier skeleton

- Started work session.
- Created `Documentation/product-and-technical-dossier.md` with empty headings for all brief-required dossier parts, to be filled in manually.
- Recorded this session in `AiTranscripts/2026-08-31T2001+02_dossier-skeleton.md`.

## 2026-08-31 20:35 (UTC+2) — Session: baseline scope from Whatsapp Content Distribution

- Recorded Whatsapp Content Distribution as the reference consumer (title only in docs; local path in Cursor memory only).
- Filled `Documentation/product-and-technical-dossier.md` section 1.7 with in-scope, compatibility contract, out-of-scope, and exercise success criteria so the design cannot grow past a drop-in replacement for that system's manifest.
- Recorded this session in `AiTranscripts/2026-08-31T2035+02_baseline-scope.md`.

## 2026-08-31 20:40 (UTC+2) — Note: video duration and file size from provider

- Added a deferred note in dossier section 1.7: duration and file size should later be sourced from the video provider, not maintained manually in data/portal. Not to be solved in this iteration.
- Recorded this session in `AiTranscripts/2026-08-31T2040+02_video-metadata-note.md`.

## 2026-08-31 20:45 (UTC+2) — Note: authenticate via Alpha Identity Provider

- Recorded intent in dossier section 1.7: API/portal auth uses Alpha's existing self-hosted Duende IdentityService as standard OAuth. No work on the IDP. Dev discovery only. Client configuration later in the build.
- Pointer added under section 2.11.
- Recorded this session in `AiTranscripts/2026-08-31T2045+02_idp-auth-intent.md`.

## 2026-08-31 20:49 (UTC+2) — Baseline user stories and journeys

- Added baseline user stories in dossier section 1.4 (API: hosts/helpers, potential guests, evaluator; Portal: Alpha staff / administrator), Connextra form, for manual refinement.
- Added mermaid journeys and flowcharts in section 1.5, split by API user and Portal user.
- Recorded this session in `AiTranscripts/2026-08-31T2049+02_user-stories-journeys.md`.

## 2026-08-31 21:03 (UTC+2) — Verified mermaid journey syntax

- Checked dossier journey diagrams against the Mermaid user-journey spec. Syntax is valid; charts not converted to another type.
- Markdown preview not rendering journeys while the Mermaid plugin does is a preview-renderer issue, not invalid diagram source.
- Recorded this session in `AiTranscripts/2026-08-31T2103+02_mermaid-journey-syntax.md`.

## 2026-08-31 21:17 (UTC+2) — User-story refinements (API)

- Left mermaid journeys unchanged.
- Removed US-API-07 (evaluator API read) to keep API stories simple; original inclusion was from the brief's dedicated evaluator account, not a product persona.
- Candidate already updated US-API-04, dropped US-API-05, and unified guest/host API stories.
- Recorded client-credentials as later-auth context only under section 1.7 (not designed now).
- Recorded this session in `AiTranscripts/2026-08-31T2117+02_api-story-refine.md`.

## 2026-08-31 21:25 (UTC+2) — Session ended: product/experience baseline paused

- Noted later portal authorization as read vs write scopes (not a separate evaluator API user). US-POR stories left as reviewed.
- Noted OpenAPI/Swagger generation later in the build for API docs, testing, and demo flexibility. Not designed now.
- Ended this work session.
- Recorded this session in `AiTranscripts/2026-08-31T2125+02_session-end.md`.

## 2026-08-31 21:27 (UTC+2) — Day summary

- Wrote `TimeTracking/2026-08-31-day-summary.md`: counted session windows only (11:31–11:37 and 20:01–21:25), not the 11:37–20:01 gap.
- Rough spend today: about 1 h 32 min including this wrap (1 h 30 min without it).
- Recorded this session in `AiTranscripts/2026-08-31T2127+02_day-summary.md`.

## 2026-09-01 06:14 (UTC+2) — Session started: requirements baseline

- Started work session.
- Populated dossier section 1.6 with a best-effort requirements summary from purpose, stories, journeys, scope, and brief constraints (functional API/portal/exercise, NFRs, explicit non-requirements).
- Recorded this session in `AiTranscripts/2026-09-01T0614+02_requirements-baseline.md`.

## 2026-09-01 06:17 (UTC+2) — Getting Started marked stretch

- Noted in dossier sections 1.6 and 1.7 that Getting Started content (payload, portal edit, training videos) is a stretch goal if time remains; not required for exercise success.
- Recorded this session in `AiTranscripts/2026-09-01T0617+02_getting-started-stretch.md`.

## 2026-09-01 06:36 (UTC+2) — Architecture baseline

- Recorded baseline architecture in dossier sections 1.8 and 2: C# hexagonal API, Postgres, likely React portal, two-repo operating model, consume existing C# IDP and JS/Astro Whatsapp Content Distribution. To be refined.
- Recorded this session in `AiTranscripts/2026-09-01T0636+02_architecture-baseline.md`.

## 2026-09-01 06:40 (UTC+2) — Architecture review

- Reviewed the current dossier baseline for outdated, risky, or unlikely-to-work architecture.
- Main risks: client credentials in a static Astro client; Alpha IDP as the only live auth; drop-in vs not changing the consumer; two-repo/hexagonal/over-normalized Postgres in the 12h timebox.
- Wrote `Documentation/architecture-review-2026-09-01.md`.
- Recorded this session in `AiTranscripts/2026-09-01T0640+02_architecture-review.md`.

## 2026-09-01 06:47 (UTC+2) — Auth/BFF decisions and 1.5 wording

- Section 1.5 API journey: "client authenticates" (not host sign-in).
- Recorded: WCD needs a BFF for client credentials; not in this POC; no work outside this project.
- Demo will use existing Alpha IDP test clients in dev (clients stored in data).
- AI-assisted implementation to stay inside 12 hours (D-ARCH-06, D-ARCH-07).
- Recorded this session in `AiTranscripts/2026-09-01T0647+02_bff-idp-journeys.md`.

## 2026-09-01 06:55 (UTC+2) — Product definition baseline

- Defined a product as an Alpha course (episode series + materials), with families, variants/translations, episode Brightcove/Vimeo feeds, and product-level training videos.
- Added dossier Product definition and section 2.7 baseline model; to refine against Whatsapp Content Distribution and legacy next.
- Recorded this session in `AiTranscripts/2026-09-01T0655+02_product-definition.md`.

## 2026-09-01 06:59 (UTC+2) — Product data description: family link, separate translation vs contextualization

- Contextualized editions are their own product entries; family is a linked property.
- Translation (dubs/subs) is a separate factor from contextualization and can apply to any product, including the main family edition.
- Description only; not model design. Refine against Whatsapp Content Distribution and legacy still pending.
- Recorded this session in `AiTranscripts/2026-09-01T0659+02_product-entries.md`.

## 2026-09-01 07:03 (UTC+2) — Session paused

- Paused this work session. Candidate will return later.
- Last work: product data description (family as linked property; contextualization vs translation).
- Recorded this session in `AiTranscripts/2026-09-01T0703+02_session-pause.md`.

## 2026-09-01 08:26 (UTC+2) — Session started: data model details (manual)

- Restarted work after pause.
- Candidate will define data model details manually in `Documentation/product-and-technical-dossier.md` (section 2.6 / 2.7).
- Recorded this session in `AiTranscripts/2026-09-01T0826+02_session-restart.md`.

## 2026-09-01 08:45 (UTC+2) — Review of schema design vs design paradigm

- Evaluated section 2.7 schema and data dictionary against product definition, WCD projection, and scope freeze. Findings in chat only; dossier not edited.
- Recorded this session in `AiTranscripts/2026-09-01T0845+02_schema-review.md`.

## 2026-09-01 09:10 (UTC+2) — Schema decisions: dubs/subs, identifiers, cuts

- Brightcove/Vimeo: playback is extra audio/text tracks on one video; download-first MP4s are not a second track on the same file. Dossier not yet modeling dub-as-product.
- `default_audience` risks discussed in chat; column not added.
- Removed from product/dictionary: slug, sequence, legacy_ref, published_at. Removed product status and product_market status. Removed dictionary-only extras (views, is_active except language, asset mime/sequence/legacy_ref).
- Captured: grouping is API/UI; asset_market exact match; product_item not shared; tag category normalised + hard-coded UI; market country via ACL lookup.
- Recorded this session in `AiTranscripts/2026-09-01T0910+02_schema-decisions.md`.

## 2026-09-01 09:26 (UTC+2) — MyAlpha graphs vs exercise scope (feedback)

- Candidate clarified MyAlpha: no “contextualization” name; three graphs (different course / country shadow / RTML copy). Dubs/subs live on episode videos (Brightcove language flags; Vimeo had separate resource URLs).
- Chat-only feedback: new product for different video+audio content; not a new product for English+French dub/sub. Country listing via market ACL, not field overrides. RTML copy posts out of this exercise.
- Dossier not edited.
- Recorded this session in `AiTranscripts/2026-09-01T0926+02_myalpha-graphs-scope.md`.

## 2026-09-01 09:31 (UTC+2) — Dossier updated: product vs dub vs country

- Locked product definition: new product for different films; dub/sub on episode assets (Brightcove language flag); country is market listing + ACL; WCD variantKey is a query the API assembles.
- Aligned stories, requirements, scope, D-DOM-01/02, journeys, and section 2.7 dictionary. No default_audience column. RTML and country shadow-overrides out of scope.
- Recorded this session in `AiTranscripts/2026-09-01T0931+02_dossier-model-lock.md`.

## 2026-09-01 09:46 (UTC+2) — Portal React Admin, BFF adapters, structure for review

- Portal: React Admin, TypeScript, React, Vite; full CRUD on catalog models; OIDC via this product's portal BFF (not a SPA secret).
- C#: two inbound adapters (Host.Bff CRUD + Host.WebApi read-only), MyAlpha BFF-shaped modularisation. IDP consume-only; no role claims.
- Cursor rules: IDP and MyAlpha BFF named in docs; local paths AI-only; do not change those repos.
- Dossier sections 1.7–1.8 and 2.1–2.5 / 2.8–2.12 updated for review. No Solution code yet.
- Recorded this session in `AiTranscripts/2026-09-01T0946+02_portal-bff-structure.md`.

## 2026-09-01 10:00 (UTC+2) — ApplicationRoot one process; Product.sln naming

- Solution/C# named `product` (`Product.sln`). Entrypoint `Product.ApplicationRoot` (one process). BFF and WebApi are adapter services, not second hosts.
- Persistence: `Product.ProductStore` (Postgres repository). No `Driven.*` / `Driver.*` project names.
- Structure inspired by Core Services Profile ApplicationRoot (no double nest, no inner Kestrel reverse-proxy). Cursor rule added; path AI-only.
- Dossier §1.8 and §2.1–2.5 / 2.8–2.12 updated. No Solution code yet.
- Recorded this session in `AiTranscripts/2026-09-01T1000+02_applicationroot-product-structure.md`.

## 2026-09-01 10:03 (UTC+2) — Session paused: meetings and work

- Paused this work session. Candidate will return later.
- Last work: C# structure locked in dossier as one `Product.ApplicationRoot` process (`product` / `product-portal`); BFF and WebApi as adapter services; `Product.ProductStore` repository. No Solution code yet.
- Recorded this session in `AiTranscripts/2026-09-01T1003+02_session-pause.md`.

## 2026-09-01 11:03 (UTC+2) — Drop Application project; tests; scopes; privacy

- Removed `Product.Application`. Domain + ProductStore + adapters only. Per-module unit tests; acceptance on Bff, WebApi, ApplicationRoot.
- Auth: existing `alpha.idp.read` (WebApi client credentials) and `alpha.idp.readwrite` (portal OIDC). Portal allow-list stub of IDP accounts; no IDP roles; no user table.
- Filled dossier §3.1, §3.3–3.6. IDP Cursor rule updated for scopes and allow-list.
- Recorded this session in `AiTranscripts/2026-09-01T1103+02_tests-auth-privacy.md`.

## 2026-09-01 11:10 (UTC+2) — Observability: Application Insights + stdout

- Standard Alpha Application Insights in ApplicationRoot. Connection string from env; if unset, logs to stdout.
- Custom events for alert candidates (allow-list deny, auth failed, catalog miss, product saved). No tokens/PII in logs. Alert rules later.
- Dossier §3.2, §2.10–2.12, NFR-08. Timeline also covers the 11:03 session work already logged above (tests, scopes, privacy).
- Recorded this session in `AiTranscripts/2026-09-01T1110+02_observability-appinsights.md`.

## 2026-09-01 11:13 (UTC+2) — Full dossier review (gaps and concerns)

- Reviewed `Documentation/product-and-technical-dossier.md` against locked decisions and the brief. Findings in chat only; dossier not edited.
- Main gaps: empty section 4; thin 2.5/2.6/2.9; journeys and WCD-as-runtime-client stale vs client-credentials POC; country ACL and drop-in HTTP contract unspecified.
- Recorded this session in `AiTranscripts/2026-09-01T1113+02_dossier-full-review.md`.
## 2026-09-01 11:32 (UTC+2) — Dossier: no WCD drop-in; wwwroot demo; baseline contract

- Journeys: authenticate (not sign-in); portal path includes Product.Bff OIDC then cookie.
- Not scoring against Whatsapp Content Distribution. ApplicationRoot wwwroot demo page with buttons at `/`; portal on a separate path.
- Removed Deferred to data design and US-API-05 (FR-API-05 remains).
- API is a concept catalog: baseline contract is all product data in the system; country–market ACL coded at implementation; audience stays many-to-many; language omitted falls back to product content_language.
- Demo IDP clients in env after scaffold. Evaluator vs admin labels ignored until problematic.
- Recorded this session in `AiTranscripts/2026-09-01T1132+02_dossier-review-decisions.md`.
## 2026-09-01 11:40 (UTC+2) — Scaffold Product.sln (API)

- Created `Solution/product/Product.sln`: ApplicationRoot, Domain, Bff, WebApi, ProductStore, and paired unit/acceptance tests.
- ApplicationRoot plugs adapters and serves wwwroot demo at `/`. No catalog or IDP implementation yet. `.env.example` for client details after scaffold.
- Recorded this session in `AiTranscripts/2026-09-01T1140+02_scaffold-product-api.md`.

## 2026-09-01 11:50 (UTC+2) — Postgres schema + smoke test

- Added `Solution/Data/product_schema.sql` from dossier section 2.7 (tables, uniques, CHECKs, composite asset FK). No country-market ACL table; audience remains many-to-many.
- Added `Solution/Data/product_schema_smoketest.sql` to assert that implementation. Ran it against a throwaway database: PASS.
- Dossier section 2.6 points at these files.
- Recorded this session in `AiTranscripts/2026-09-01T1150+02_product-schema-sql.md`.

## 2026-09-01 11:58 (UTC+2) — ProductStore plan, product_service DB, DBHub MCP

- Wrote `Documentation/product-store-build-plan.md` (IProductStore, Npgsql, tests, build order).
- Created Postgres database `product_service`, applied `product_schema.sql`, smoke test PASS.
- Added project DBHub MCP (`.cursor/mcp.json`) and a Cursor rule for schema/store work.
- Recorded this session in `AiTranscripts/2026-09-01T1158+02_product-store-plan-dbhub.md`.

## 2026-09-01 12:08 (UTC+2) — ProductDbContext and ProductRepository CRUD

- Domain entities for the 10 catalog tables. Port is `IProductRepository`; store implements `ProductRepository` on EF Core `ProductDbContext` (no migrations).
- `AddProductStore` requires `ConnectionStrings:ProductStore`. Local `product_service` DSN in ApplicationRoot appsettings.
- Recorded this session in `AiTranscripts/2026-09-01T1208+02_product-repository.md`.

## 2026-09-01 12:25 (UTC+2) — Product.WebApi GET catalog routes

- Mapped unauthenticated GET routes on Product.WebApi under `/catalog` (`Endpoints/` handlers + `Routes.MapApi` with OpenAPI `.Produces` chaining). Auth left off for local testing.
- Product payloads include family, tags, markets, items, and language-selected assets. Domain `CountryMarketAcl` and `AssetLanguageSelector` used for country filter and language fallback.
- Recorded this session in `AiTranscripts/2026-09-01T1225+02_webapi-get-routes.md`.

## 2026-09-01 12:19 (UTC+2) — Name this Product, not Catalog

- Added always-apply Cursor rule `.cursor/rules/product-naming.mdc`. New writing and identifiers use Product, not Catalog.
- WebApi route prefix is `/product`; snapshot/mapper types renamed. Dossier §2.9 paths updated.
- Recorded this session in `AiTranscripts/2026-09-01T1219+02_product-not-catalog.md`.

## 2026-09-01 12:30 (UTC+2) — Domain models as sealed records

- Catalog types live in `Product.Domain/Models/` as sealed records (`init`). Repository assigns ids with `with`.
- `CatalogSnapshot` is a sealed record. WebApi payloads were already records.
- Recorded this session in `AiTranscripts/2026-09-01T1230+02_domain-models-records.md`.

## 2026-09-01 12:35 (UTC+2) — ProductWebApiAdapter orchestrates the module

- Added `ProductWebApiAdapter` to register WebApi services and map `/product` on ApplicationRoot (same host). Extensions stay as the Program façade.
- Recorded this session in `AiTranscripts/2026-09-01T1235+02_webapi-adapter.md`.

## 2026-09-01 12:50 (UTC+2) — Scoped ProductStore on ApplicationRoot

- ApplicationRoot `AddProductModules` registers ProductStore first so Bff and WebApi can inject `IProductRepository`.
- `ProductStoreAdapter` uses scoped `ProductDbContext`, scoped options, and scoped repository (not singleton).
- Recorded this session in `AiTranscripts/2026-09-01T1250+02_scoped-product-store.md`.

## 2026-09-01 12:55 (UTC+2) — Session paused

- Paused this work session. Candidate will return later.
- Last work: ApplicationRoot `AddProductModules` plugs ProductStore first; `ProductStoreAdapter` registers scoped `ProductDbContext`, options, and `IProductRepository`.
- Recorded this session in `AiTranscripts/2026-09-01T1255+02_session-pause.md`.

## 2026-09-01 13:37 (UTC+2) — Session started: Rider plus CLAUDE.md

- Restarted work after pause. Candidate switching to Rider for manual coding and tweaks.
- Added root `CLAUDE.md` from existing Cursor rules, plus always-apply rule `.cursor/rules/claude-md-sync.mdc` so `.cursor/rules/` and `CLAUDE.md` stay in sync.
- Recorded this session in `AiTranscripts/2026-09-01T1337+02_session-start-claude-md.md`.

## 2026-09-01 14:00 (UTC+2) — Bearer authentication on Product.WebApi

- Added `Product.WebApi/Authentication`: named JWT bearer scheme `ProductApiBearer` plus policy `ProductApiRead` (authenticated caller with scope `alpha.idp.read`). Scheme is named, not default, and pinned by the policy so a later Product.Bff cookie session cannot satisfy a `/product` route.
- `Routes.MapApi` applies `RequireAuthorization` on the `/product` group so no route can be left anonymous by omission. `Program` gained `UseAuthentication` / `UseAuthorization`.
- `Identity` settings in `appsettings.json`: `Authority`, `Audience` (`{authority}/resources`, the IDP static audience), `RequireHttpsMetadata`, `ReadScope`. Adapter throws when `Identity:Authority` is missing.
- Added `Product.WebApi/products.http`, `http-client.env.json` (client-credentials OAuth2 block, `local` and `local-https`), and gitignored `http-client.private.env.json` for the demo client id/secret.
- WebApi acceptance tests split: stubbed bearer for route behaviour, real scheme for anonymous / malformed-token 401 and wrong-scope 403. Full suite green (37 tests). Verified live: `/` 200, `/product/languages` 401.
- Recorded this session in `AiTranscripts/2026-09-01T1400+02_webapi-bearer-auth.md`.

## 2026-09-01 14:05 (UTC+2) — products.http uses env bearer token

- Product API calls in `products.http` now send `Authorization: Bearer {{$auth.token("product_clientcredentials_read")}}`, matching the OAuth2 block in `http-client.env.json` (no spaces inside the variable). Anonymous and garbage-token cases stay unauthenticated on purpose.
- Recorded this session in `AiTranscripts/2026-09-01T1405+02_products-http-bearer.md`.

## 2026-09-01 14:08 (UTC+2) — Swagger UI for Product.WebApi

- Swashbuckle on Product.WebApi: OpenAPI at `/swagger/v1/swagger.json`, UI at `/swagger`. Bearer Authorize (paste a client-credentials token). Document lists `/product` routes only.
- `UseProductWebApi` maps Swagger before authentication so the UI stays anonymous. Demo page and `products.http` link to it. `dotnet test` Product.sln green.
- Recorded this session in `AiTranscripts/2026-09-01T1408+02_swagger-ui.md`.

## 2026-09-01 14:21 (UTC+2) — Product.Bff CRUD under /api

- Product.Bff adapter: Duende BFF, named cookie/OIDC schemes, allow-list stub, `/bff` management + `/api` CRUD. GET handlers reused from Product.WebApi (auth on the BFF group: `readwrite` + allow-list). POST/PUT/DELETE in `Endpoints/Writes.cs`.
- Empty `Bff:ClientId` skips OIDC so tests start. CSRF `X-CSRF: 1`. Store gets are `AsNoTracking` so updates can attach.
- Recorded this session in `AiTranscripts/2026-09-01T1421+02_bff-crud.md`.

## 2026-09-01 14:32 (UTC+2) — Swagger document for Portal BFF

- Swagger UI at `/swagger` now has two documents: Product API (`/swagger/v1/swagger.json`, bearer) and Portal BFF (`/swagger/bff/swagger.json`, `/api` + `X-CSRF: 1`). Cookie Try it out uses same-origin credentials after `/bff/login`.
- Recorded this session in `AiTranscripts/2026-09-01T1432+02_swagger-bff-doc.md`.

## 2026-09-01 14:38 (UTC+2) — Scaffold product-portal

- Initialized `Solution/product-portal` with Vite `react-ts`, **pnpm**, React Admin, and Vitest. No extra direct packages (`ra-data-simple-rest` not added; custom same-origin `dataProvider` with cookie + `X-CSRF: 1`).
- React Admin resources for BFF models; `/bff/login` auth; Vite `base` `/admin/` and proxy to ApplicationRoot. `pnpm test` and `pnpm build` green.
- Recorded this session in `AiTranscripts/2026-09-01T1438+02_scaffold-product-portal.md`.

## 2026-09-01 14:59 (UTC+2) — Portal BFF OIDC login

- Portal login is a full-page `/bff/login?returnUrl=/admin/` (not `/admin/bff/login`). Vite 302s the SPA path and proxies `/bff` + `/signin-oidc` with `X-Forwarded-*` so the IDP `redirect_uri` is `http://localhost:5173/signin-oidc`.
- Product.Bff OIDC: authorization code + PKCE, callback `/signin-oidc`, Lax correlation cookies in Development. ApplicationRoot honors forwarded headers in Development.
- Recorded this session in `AiTranscripts/2026-09-01T1459+02_portal-bff-oidc-login.md`.

## 2026-09-01 15:07 (UTC+2) — Session pause

- Paused after portal BFF OIDC login wiring. Next: restart Product.ApplicationRoot and confirm the IDP round-trip from Vite (`/bff/login` → `dev.auth.alpha.org` → `/signin-oidc`).
- Recorded this session in `AiTranscripts/2026-09-01T1507+02_session-pause.md`.

## 2026-09-01 16:33 (UTC+2) — Session start: Vite HTTPS and Bff:CountryCode

- Kept the candidate's Vite TLS + `X-Forwarded-Proto: https` (certs gitignored). Cert load is skipped when files are missing so test/build still run.
- `Bff:CountryCode` on the authorize request: production `global` (`appsettings.json`), Development `za` (IDP dev cannot use the global identifier). Config override wins; code fallback matches those defaults.
- Recorded this session in `AiTranscripts/2026-09-01T1633+02_session-start-countrycode.md`.

## 2026-09-01 16:39 (UTC+2) — Portal dashboard at /admin

- React Admin `dashboard` at `/admin/`: authenticated `GET /api/products` via the BFF cookie, rendered as JSON. Vite redirects `/admin` to `/admin/`.
- Recorded this session in `AiTranscripts/2026-09-01T1639+02_portal-products-dashboard.md`.

## 2026-09-01 16:49 (UTC+2) — Portal SPA at origin root

- Removed Vite `base` `/admin/` and React Admin `basename="/admin"`. The router was matching URL `/` against basename `/admin` and rendering nothing. Dev portal is `https://localhost:5173/`.
- Auth provider aligned with React Admin required methods; logout only hits `/bff/logout` when a BFF session exists. `wwwroot` on ApplicationRoot is unchanged.
- Recorded this session in `AiTranscripts/2026-09-01T1649+02_portal-root-basename.md`.

## 2026-09-01 16:58 (UTC+2) — Portal FK names and picker modal

- Resource lists resolve foreign keys to related names (`ReferenceField` + `recordLabel`). Create/edit open a native `<dialog>` list picker instead of typing ids. Product save still sends the table row, not the nested GET payload.
- Recorded this session in `AiTranscripts/2026-09-01T1658+02_portal-fk-picker.md`.

## 2026-09-01 17:04 (UTC+2) — Day summary (time left)

- Counted session windows only (same method as 2026-08-31). Today through 17:04: about 6 h 19 min. Exercise total including yesterday: about 7 h 51 min. About 4 h 9 min left in the 12-hour envelope for deploy, refinement, and final docs.
- Wrote `TimeTracking/2026-09-01-day-summary.md`.
- Recorded this session in `AiTranscripts/2026-09-01T1704+02_day-summary.md`.

## 2026-09-01 17:04 (UTC+2) — Portal products home and BFF logout

- Removed the dashboard. After login, `/` opens the products list (`/#/products`).
- Logout navigates to Product.Bff `/bff/logout?sid=…` from `bff:logout_url` so Duende can clear the cookie and run IDP end-session (`/signout-callback-oidc`). Restart ApplicationRoot so the OIDC sign-out `PostLogoutRedirectUri` change is loaded.
- Recorded this session in `AiTranscripts/2026-09-01T1707+02_portal-logout-products.md`.

## 2026-09-01 17:12 (UTC+2) — Portal 403 page instead of auto-logout

- Allow-list miss no longer flashes the empty portal then logs out. `checkAuth` waits on `GET /api/languages` behind a spinner (no 1s delay). 403 shows an Unauthorized page with Log out; 401 still goes to login.
- Recorded this session in `AiTranscripts/2026-09-01T1712+02_portal-403-allowlist.md`.

## 2026-09-01 17:17 (UTC+2) — Session pause

- Paused. Portal CRUD, FK names/picker, products as home, BFF logout (`sid`), and allow-list 403 page are in place. Next work is deployments and documentation (outside this session).
- Recorded this session in `AiTranscripts/2026-09-01T1717+02_session-pause.md`.

## 2026-09-02 13:53 (UTC+2) — Schema without pgcrypto

- Dropped `CREATE EXTENSION pgcrypto` from `product_schema.sql`. UUID defaults use core `gen_random_uuid()` (PostgreSQL 13+ / Azure Flexible Server). Re-applied locally and smoke-tested.
- Recorded this session in `AiTranscripts/2026-09-02T1353+02_azure-pg-schema.md`.

## 2026-09-02 14:27 (UTC+2) — Pause tracking

- Paused tracking. Last AI work this session: schema without `pgcrypto` for Azure Flexible Server. Deployment continues outside this session.
- Recorded this session in `AiTranscripts/2026-09-02T1427+02_pause-tracking.md`.

## 2026-09-02 15:29 (UTC+2) — Dossier §4 review and repo README

- Reviewed dossier additions (mainly §4, plus §2.5/§2.12 and §3.6 data limitations). Added a root `README.md`: one-paragraph product summary, directory map, link to the dossier.
- Recorded this session in `AiTranscripts/2026-09-02T1529+02_dossier-readme.md`.

