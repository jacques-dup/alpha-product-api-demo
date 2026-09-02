# Session 2026-09-01T1240+02

**Session timestamp:** 2026-09-01T12:40+02:00

## User input

look at Data/product_shema.sql and its relevant smoke test. write me a query called seed_product_data_example.sql that I can run to seed an example product into my database for testing.

## Assistant output

Added `Solution/Data/seed_product_data_example.sql`. It upserts languages (`en`, `fr`), markets (`za`, `gb`, `ssa`, `lat`), the Alpha Film Series family, and tags, then replaces product `alpha-film-series` with two episodes, one optional training item, EN/FR main videos, hero/material assets, and a ZA-only supporting clip. Re-runnable. Dossier §2.6 now points at the seed file.
