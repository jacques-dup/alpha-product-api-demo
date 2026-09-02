# Product portal

Staff UI for product data. **React Admin** on TypeScript, React, and Vite. Talks to **Product.Bff** on the same origin (`/api`, cookie + `X-CSRF: 1`). There is no client secret in the SPA.

Use **pnpm** (`pnpm-lock.yaml`). Direct packages are the dossier stack only: Vite, React, TypeScript, React Admin, Vitest, plus the official Vite `react-ts` template tooling.

Dev server is the **origin root** (`https://localhost:5173/`). ApplicationRoot `wwwroot` stays on the API host only; Vite does not proxy it.

## Scripts

| Command | What it does |
| --- | --- |
| `pnpm dev` | Vite at `https://localhost:5173/`. Proxies `/api`, `/bff`, and `/signin-oidc` to ApplicationRoot (`https://localhost:7127` by default; override with `VITE_APPLICATION_ROOT`). Local TLS: `dotnet dev-certs https --export-path ./certs/localhost.pem --format Pem --no-password` (certs are gitignored). |
| `pnpm build` | Production bundle (`base` `/`). |
| `pnpm preview` | Serve that bundle. |
| `pnpm test` | Vitest (node). |
| `pnpm lint` | Oxlint (template). |

## Auth

Follows the React Admin [auth provider](https://marmelab.com/react-admin/SecurityGuide.html) contract: `login`, `logout`, `checkAuth`, `checkError`, `getIdentity`. `canAccess` mirrors the Product.Bff **allow-list** (not IDP role claims): after `/bff/user`, the SPA probes `GET /api/languages`. Allowed accounts see the portal. A 403 stays signed in and shows a **403 Unauthorized** page with a Log out button. A 401 still goes to login. `loading` uses a spinner with no delay so the empty shell does not flash during that check.

Sign-in is a **full-page** navigation to **`/bff/login`** (Product.Bff), not a React Admin form. The custom `loginPage` only starts that challenge ([custom login page](https://marmelab.com/react-admin/SecurityGuide.html)).

Flow:

1. Portal `checkAuth` calls `GET /bff/user`. 401 shows the login page, which replaces the location with `/bff/login?returnUrl=/`.
2. Product.Bff challenges OIDC (authorization code + PKCE) and redirects the browser to the IDP at `https://dev.auth.alpha.org`.
3. After login, the IDP redirects to **`/signin-oidc`** with the authorization code. Product.Bff exchanges the code, stores tokens in the cookie session, and sends you back to `/`. `checkAuth` then probes `GET /api/languages` (allow-list). A spinner stays up until that returns. Allow-listed accounts open **products** (`/#/products`). A 403 shows the Unauthorized page (session kept until Log out).
4. `/api` calls send that cookie plus `X-CSRF: 1`. The SPA never holds the access token or client secret.
5. Logout is a full-page navigation to Product.Bff **`/bff/logout?sid=…`**. The `sid` comes from the `bff:logout_url` claim on `/bff/user` (Duende CSRF). That clears the BFF cookie and sends the browser to the IDP end-session endpoint, then **`/signout-callback-oidc`**.

The demo BFF client (ids in ApplicationRoot user-secrets) must allow these redirect URIs:

- `https://localhost:5173/signin-oidc` (Vite)
- `https://localhost:7127/signin-oidc` (ApplicationRoot HTTPS)
- matching `.../signout-callback-oidc` post-logout URIs

ApplicationRoot needs `Bff:ClientId` / `Bff:ClientSecret` and `Portal:AllowList:0` (IDP subject or email). Empty ClientId skips OIDC so tests can start.

`Bff:CountryCode` is sent on the authorize request for IdentityUI client customization. Production is `global` (IDP default if omitted). IDP **dev** cannot use that identifier, so Development falls back to `za` (`appsettings.Development.json`). Override with `Bff:CountryCode` if needed.

After sign-in, `/` redirects to the **products** list (`/#/products`). Other resources are `/#/items`, `/#/languages`, and so on.

## Data

The `dataProvider` is a small same-origin fetch adapter (not `ra-data-simple-rest`). Languages and markets use `code` as React Admin `id`. Join rows use `productId_tagId` / `productId_marketCode` / `assetId_marketCode`. Lists resolve foreign keys to the related name (family, product, tag, market, language code) via `ReferenceField`. Create and edit use a native `<dialog>` picker (`ReferencePickerInput`) instead of typing ids. Product saves send the table row only (`familyId`, `code`, `title`, `summary`, `description`, `contentLanguage`) because GET `/api/products` returns the nested payload.
