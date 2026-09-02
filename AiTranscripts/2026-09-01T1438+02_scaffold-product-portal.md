# Session 2026-09-01T1438+02

**Session timestamp:** 2026-09-01T14:38+02:00

## User input

Lets move on to scaffolding the Portal component. Look at the dossier and initialize the boilerplate for the portal. Alos note that we should use pnpm over npm and that we should not add any other packages or framework than what was originally listed. If the model wants to suggest a package, I need to verify its use first every time before it is included in the build.

## Assistant output

The portal boilerplate is in `Solution/product-portal`: Vite + React + TypeScript + React Admin + Vitest, installed with **pnpm** (`pnpm-lock.yaml` only). Direct dependencies stay on that list; I did not add `ra-data-simple-rest` or anything else.

What is wired:

- React Admin at `/admin/` with resources for the Product.Bff models (lists/edits are guessers; creates are SimpleForm fields because React Admin 5 has no CreateGuesser).
- Sign-in via `/bff/login`. API calls send the BFF cookie and `X-CSRF: 1`.
- A small custom `dataProvider` (same-origin fetch). Languages/markets use `code` as `id`; join rows use composite ids.
- Dev proxy to ApplicationRoot (`https://localhost:7127` by default). `pnpm test` and `pnpm build` pass.

From here, if a library looks useful (`ra-data-simple-rest`, jsdom, extra MUI, and so on), I will ask before adding it.
