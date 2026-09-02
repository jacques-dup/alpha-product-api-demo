# AI Transcript — 2026-08-31 20:49 (UTC+2)

## User

Using best practice definitions, include a baseline for our user stories that might come up. I'll refine them manually. Similarly create mermaid diagrams to represent the user journeys. split them up by API user and Portal user as defined in the Users section.

## Assistant

Baseline stories and journeys are in `Documentation/product-and-technical-dossier.md` sections 1.4 and 1.5. Refine in place.

**User stories** (Connextra: As a … I want … so that …)

- API — hosts/helpers: fetch current variant, episodes/downloads, Getting Started, language and country filters, OAuth via Alpha IDP, evaluator read
- API — guests: same read, marked potential / not this iteration
- Portal — Alpha staff: IDP sign-in, list, filter, create, edit, video host/URL, persist to API, admin write vs evaluator read

**Journeys** (mermaid, split as requested)

- API user: journey + flowchart (client authenticates, resolves variant, host uses materials; guest is optional)
- Portal user: journey + flowchart (staff signs in, finds variant, saves, API serves the update)
