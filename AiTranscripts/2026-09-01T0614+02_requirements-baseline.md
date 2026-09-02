# AI Transcript — 2026-09-01 06:14 (UTC+2)

## User

populate teh requirements section with a best effort summary of what we have so far

## Assistant

Section 1.6 in `Documentation/product-and-technical-dossier.md` is now a best-effort requirements baseline from what is already agreed. Not a locked spec.

- **API:** fetch variant by course grain, payload (episodes, video, Getting Started), filter by course properties, OAuth (client credentials later), drop-in manifest shape, reads reflect portal saves
- **Portal:** IDP sign-in, list/filter/create/edit, video host/URL, persist, read vs write scopes later, mobile-responsive
- **Exercise:** live deploy, staff-edit then API-read workflow, small representative seed
- **NFR:** persistence, consume existing IDP (dev only), no larger data model, no secrets in repo
- **Not this iteration:** consumer rewrite, live video platforms, provider-sourced duration/size, Swagger, client/grant design
