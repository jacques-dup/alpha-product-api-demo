# Session 2026-09-01T1459+02

**Session timestamp:** 2026-09-01T14:59+02:00

## User input

http://localhost:5173/admin/bff/login does nothing much on its own. Lets set up the login logic to do the authorization flow for the bff client. Which will have us redirect to the issuer dev.auth.alpha.org to be presented with the login screen. upon login we will be redirected back to the singin-oidc endpoint witht the authentication token that we can use for our requests.

## Assistant output

`/admin/bff/login` is a React Admin route, so it never reached Product.Bff. Login is now a full-page navigation to `/bff/login?returnUrl=/admin/`. Vite 302s `/admin/bff/login` there.

That endpoint challenges OIDC (authorization code + PKCE) and sends the browser to `https://dev.auth.alpha.org`. After sign-in, the IDP returns to `/signin-oidc` with the code. Product.Bff exchanges it, keeps the tokens in the cookie session, and returns you to `/admin/`. `/api` then uses that cookie plus `X-CSRF: 1` — the SPA still does not hold the access token.

Restart **Product.ApplicationRoot** so Development forwarded headers and OIDC correlation cookies load. The BFF demo client must allow `http://localhost:5173/signin-oidc` (Vite) and `https://localhost:7127/signin-oidc` (Kestrel), plus the matching sign-out callbacks.
