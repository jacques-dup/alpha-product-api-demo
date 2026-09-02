# Session 2026-09-01T1132+02

**Session timestamp:** 2026-09-01T11:32+02:00

## User input

For contridictions:
update the wording of "signing in" to just be "authenticate" and include the bff step for portal
We won't be measuring this against the whatsapp content site. We can add a wwwroot in the ApplicationRoot to serve at the root domain for the project and buil a single page with some buttons to demo the api with, similar to what we've done for our other bffs and IDP.
Remove the Deferred to data design addition.
Remove US-API-05 in favour of the functional requirement.

For design questions
Ignore the drop in replacement spec. We'll build this API as a concept and adapt the consuming service only if this is seen as a viable product.
We'll create the country-market acl as we code.
instead of following the manifest json structure, we will start with just a baseline contract that includes all the data for the product we have in the system. This contract can be adjusted in the future if we decide to use the service
Audience can remain as is for now - the api can handle the filtering. Having more than one audience will be a business decision for later. We don;t want to tighten the control without additional consideration.
content_language can fall back to product if not specified.
I am able to add IDP clients easily for this demo. I'll specify its details in my env files once the project is scaffolded.
The evaluator definition is semantics. Ignore that for now untill it becomes problematic.

## Assistant output

Applied your review decisions in `Documentation/product-and-technical-dossier.md`.

**Contradictions**
- Journeys and stories now say **authenticate**. The portal path includes **Product.Bff** (OIDC, then cookie session).
- This POC is **not** measured against Whatsapp Content Distribution. **ApplicationRoot wwwroot** serves a **single demo page with buttons at `/`** (same family as other Alpha BFFs and IDP). The React Admin SPA is on a separate path (e.g. `/admin`).
- Removed **Deferred to data design**.
- Removed **US-API-05**. Client credentials stay as **FR-API-05**.

**Design**
- Drop-in / manifest JSON is out. The read API is a **concept catalog**: **baseline contract = all product data we store**, adjustable later if a consumer is adopted.
- Country–market ACL is **created as we code**, not a dossier table.
- Audience stays many-to-many; the API filters. No unique-audience constraint.
- Omitted language **falls back to product `content_language`**.
- Demo IDP **clients** go in **env after scaffold**. No IDP service/role/policy work.
- Brief evaluator vs administrator labels are **ignored until they become a problem**.

Cursor rules for Whatsapp Content Distribution and IDP were updated to match. Timeline and this transcript recorded.
