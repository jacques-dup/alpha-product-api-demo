# AI Transcript — 2026-09-01 06:36 (UTC+2)

## User

This is the baseline architecture for this design. IDP is a C# service. Whatsapp content is a javascript client built with Astro. I want this solution to also be a C# API that fits within our stack, the portal we can still deliberate on but it makes sense to build the UI with a javascript frameworc, likely react. The reasoning for this is if we do move forward with this as a potential product, the team will already be familiar with the technological approach. This also means the API and the Portal will probably be 2 differentrepositories - so that the UI can be maintained by the dedicated UI team after the initial build and the API can be maintained by the API team. For the database we will use postgres. The API project will be structured using a pseudo hexagonal design approach, so that we can inteface any ports and adapters for easier maintenance and interchange. Update this in the doc as a baseline that I will refine further

## Assistant

Baseline architecture is in `Documentation/product-and-technical-dossier.md` sections 1.8 and 2. Refine in place.

- **API:** C#, pseudo-hexagonal (ports and adapters), PostgreSQL
- **Portal:** JavaScript, likely React (still open); separate repo from the API for UI team vs API team
- **Neighbours:** consume existing C# Duende IDP; Whatsapp Content Distribution stays JS/Astro and is not rebuilt
- **This exercise:** both apps may still sit under `Solution/` as two deployables until a git split is useful

Context, architecture, and component mermaid diagrams added. Data model, contracts, and hosting left as "to be refined".
