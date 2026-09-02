# Session 2026-09-01T1633+02

**Session timestamp:** 2026-09-01T16:33+02:00

## User input

ok start session. I've made some manual changes to the vite config in order to forward the auth request as https, as wel as default the client requested countryCode for the BFF in AddProductBffAuthentication. There is currently a bug in IDP dev that means we can't use the global identifier for client customization. I've set it so that we can add a "Bff:CountryCode" configuration with a fallback to za. The value should be "global" in production (which is what IDP will default to if none is specified)

## Assistant output

Session started. Your Vite HTTPS proxy (`X-Forwarded-Proto: https`, local certs) is in place. Authorize now sends `countryCode` from `Bff:CountryCode`: **za** in Development (IDP dev cannot use the global identifier), **global** in production. An explicit config value still wins. Cert load is skipped when the files are missing so `pnpm test` / `pnpm build` still run.
