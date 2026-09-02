# Session 2026-09-01T1707+02

**Session timestamp:** 2026-09-01T17:04+02:00

## User input

ok remove the dashboard page and default to the products page after login. Also, logout is not clearing the session. Ensure the bff can end the session with a logout

## Assistant output

The dashboard is gone. After login, `/` opens the products list (`/#/products`).

Logout was calling `/bff/logout` without the session id Duende requires, so Product.Bff refused to sign out and React Admin then treated logout as an in-app route. Logout now reads `bff:logout_url` from `/bff/user` and does a full navigation to `/bff/logout?sid=…`. That clears the BFF cookie and sends the browser to the IDP end-session endpoint, then `/signout-callback-oidc`.

Restart ApplicationRoot so the sign-out `PostLogoutRedirectUri` change is loaded, then use Logout from the user menu. You should land unsigned-in (IDP login if you try again), not back in the portal with the same cookie.
