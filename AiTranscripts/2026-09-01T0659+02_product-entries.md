# AI Transcript — 2026-09-01 06:59 (UTC+2)

## User

we can;t group translations and variants as one aspect of the product. the main product from the family can be translated by either dubs or subs as well. All variants can theoretically have this. translation is one factor, and contextualization ins another. A better approach that I'd like to validate is that product variants will be their own product entries, and have the family listed as a linked property instead. this is not yet model design, it is just to better sescribe the data

## Assistant

Updated as a data description only (not a schema).

- Each catalog row is a **product** (an Alpha course).
- **Family** is a linked property, not a parent of nested editions.
- **Contextualization** (e.g. Africa, Asia) = another product entry in the same family.
- **Translation** (dubs or subs) is a separate factor. The main family product can be translated; so can every contextualized product.

Refine against Whatsapp Content Distribution and legacy is still next.
