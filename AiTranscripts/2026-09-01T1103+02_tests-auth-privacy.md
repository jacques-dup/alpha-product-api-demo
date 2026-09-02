# AI Transcript — 2026-09-01 11:03 (UTC+2)

## User

ok i also removed Product.Application witht he tag of "use cases". Don't think it fits in what was said and what would be in there. We'll also add test projects for each module for unit and acceptance tests. This informs the testing section of the dossier.

For authentication we'll use the alpha.idp.read and alpha.idp.readwrite scopes that currently exist. The API will use read and the portal readwrite. We will need to stub the roles with an authentication stub where we can use a hard coded list of approved IDP accounts to grant access to the portal instead of full role based access. This should inform the security model and the threat/risk analysis section of the dossier.

No user data will be stored in the service, only products data. User tokens will be used and fall into our existing data privacy policies adopted by IDP. The intention is to only give staff the access to log in though which also limits any user data exposure. The API itself will use bearer tokens from the client credentials flow.

## Assistant

Updated the dossier: no Product.Application; test projects per module (unit + acceptance); WebApi `alpha.idp.read` via client credentials; portal `alpha.idp.readwrite` plus an allow-list stub; privacy is catalog-only with identity remaining at IDP. Sections 3.1 and 3.3–3.6 filled. Whatsapp Content Distribution still cannot hold the client secret in this POC.
