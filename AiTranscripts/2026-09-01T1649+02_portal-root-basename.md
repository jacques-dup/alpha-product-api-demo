# Session 2026-09-01T1649+02

**Session timestamp:** 2026-09-01T16:49+02:00

## User input

I get this error from react admin on the /admin page. <Router basename="/admin"> is not able to match the URL "/" because it does not start with the basename, so the <Router> won't render anything. Verify how to implement the client according to their stadards using their documentation at https://marmelab.com/react-admin/documentation.html . Alo this is how they suggest the Auth provider be done https://marmelab.com/react-admin/SecurityGuide.html . Verify that what we've implememented on this project is done according to the standard. That being said, if the issue is that we are serving the portal on /admin instead of root and thus require additional configs, we can simplify it by keeping the portal at the root instead. It will conflict with the wwwroot we set up for the API, but the wwwroot is not intended to be proxied to from the portal, it is specific only to the API and we'll probably remove it sincve we have swagger set up now.

## Assistant output

React Admin only uses `basename="/admin"` when the browser URL is actually under `/admin`. With Vite `base: '/admin/'` the router was seeing `/`, so nothing mounted.

The portal now runs at origin root (`https://localhost:5173/`). No Vite `base` subpath, no Admin basename. Dashboard is `/`; OIDC `returnUrl` is `/`. ApplicationRoot `wwwroot` is unchanged (API host only).

Auth follows the React Admin contract: `login`, `logout`, `checkAuth`, `checkError`, `getIdentity`. Custom `loginPage` still starts `/bff/login`. `canAccess` is omitted (no IDP role claims). Logout calls `/bff/logout` only when `/bff/user` shows a session, so a failed `checkAuth` does not bounce through BFF logout.
