-- Example product seed for local testing (dossier section 2.7).
-- Run after applying product_schema.sql:
--   psql "$DATABASE_URL" -v ON_ERROR_STOP=1 -f product_schema.sql
--   psql "$DATABASE_URL" -v ON_ERROR_STOP=1 -f seed_product_data_example.sql
-- Re-runnable: upserts lookups, replaces this example product graph.
--
-- Seeded product: alpha-film-series (family alpha-film-series, copy language en).
-- Useful API checks:
--   GET /product/products/code/alpha-film-series
--   GET /product/products?courseType=alpha-film-series
--   GET /product/products?audience=adults
--   GET /product/products?country=gb          -- listed
--   GET /product/products?country=ke          -- ACL maps to ssa; listed
--   GET /product/products?country=za          -- market za; not listed
--   GET /product/products?language=fr         -- ep-01 FR dub; ep-02 falls back to en

BEGIN;

INSERT INTO language (code, is_active) VALUES
    ('en', true),
    ('fr', true)
ON CONFLICT (code) DO UPDATE
    SET is_active = EXCLUDED.is_active;

INSERT INTO market (code, kind, name) VALUES
    ('za', 'country', 'South Africa'),
    ('gb', 'country', 'United Kingdom'),
    ('ssa', 'region', 'Sub-Saharan Africa'),
    ('lat', 'region', 'Latin America')
ON CONFLICT (code) DO UPDATE
    SET kind = EXCLUDED.kind,
        name = EXCLUDED.name;

INSERT INTO product_family (id, code, name, summary, sequence) VALUES
    (
        'a0000000-0000-4000-a000-000000000001',
        'alpha-film-series',
        'Alpha Film Series',
        'A series of films that explore the Christian faith.',
        1
    )
ON CONFLICT (code) DO UPDATE
    SET name = EXCLUDED.name,
        summary = EXCLUDED.summary,
        sequence = EXCLUDED.sequence;

INSERT INTO tag (id, category, code, name, is_public, sequence) VALUES
    ('a0000000-0000-4000-a000-000000000301', 'audience', 'adults', 'Adults', true, 1),
    ('a0000000-0000-4000-a000-000000000302', 'audience', 'youth', 'Youth', true, 2),
    ('a0000000-0000-4000-a000-000000000303', 'format', 'film', 'Film', true, 1)
ON CONFLICT (category, code) DO UPDATE
    SET name = EXCLUDED.name,
        is_public = EXCLUDED.is_public,
        sequence = EXCLUDED.sequence;

DELETE FROM product WHERE code = 'alpha-film-series';

INSERT INTO product (
    id, family_id, code, title, summary, description, content_language
)
SELECT
    'a0000000-0000-4000-a000-000000000010',
    f.id,
    'alpha-film-series',
    'Alpha Film Series',
    'Fifteen films that explore the Christian faith.',
    'The Alpha Film Series creates a space to explore life, faith and God in a relaxed, non-pressured way.',
    'en'
FROM product_family f
WHERE f.code = 'alpha-film-series';

INSERT INTO product_tag (product_id, tag_id)
SELECT p.id, t.id
FROM product p
JOIN tag t ON (t.category, t.code) IN (
    ('audience', 'adults'),
    ('audience', 'youth'),
    ('format', 'film')
)
WHERE p.code = 'alpha-film-series';

INSERT INTO product_market (product_id, market_code, launched_on)
SELECT p.id, m.code, m.launched_on
FROM product p
CROSS JOIN (
    VALUES
        ('gb', DATE '2020-09-01'),
        ('ssa', NULL)
) AS m(code, launched_on)
WHERE p.code = 'alpha-film-series';

INSERT INTO product_item (
    id, product_id, kind, code, sequence, title, summary, grouping, is_optional
)
SELECT
    i.id,
    p.id,
    i.kind,
    i.code,
    i.sequence,
    i.title,
    i.summary,
    i.grouping,
    i.is_optional
FROM product p
CROSS JOIN (
    VALUES
        (
            'a0000000-0000-4000-a000-000000000101'::uuid,
            'episode',
            'ep-01',
            1,
            'Is There More to Life Than This?',
            'An invitation to explore the big questions of life.',
            NULL,
            false
        ),
        (
            'a0000000-0000-4000-a000-000000000102'::uuid,
            'episode',
            'ep-02',
            2,
            'Who Is Jesus?',
            'Who Jesus claimed to be, and why it matters.',
            NULL,
            false
        ),
        (
            'a0000000-0000-4000-a000-000000000201'::uuid,
            'training',
            'tr-01',
            1,
            'Host briefing',
            'Optional briefing for table hosts before the series starts.',
            'weekend',
            true
        )
) AS i(id, kind, code, sequence, title, summary, grouping, is_optional)
WHERE p.code = 'alpha-film-series';

INSERT INTO asset (
    id,
    product_id,
    item_id,
    role,
    kind,
    language_code,
    title,
    group_code,
    provider,
    provider_asset_id,
    stream_url,
    download_url,
    allow_stream,
    allow_download,
    duration_seconds,
    file_size_bytes
)
SELECT
    a.id,
    p.id,
    a.item_id,
    a.role,
    a.kind,
    a.language_code,
    a.title,
    a.group_code,
    a.provider,
    a.provider_asset_id,
    a.stream_url,
    a.download_url,
    a.allow_stream,
    a.allow_download,
    a.duration_seconds,
    a.file_size_bytes
FROM product p
CROSS JOIN (
    VALUES
        -- Episode 1: EN main + FR dub (same Brightcove id, different language).
        (
            'a0000000-0000-4000-a000-000000000401'::uuid,
            'a0000000-0000-4000-a000-000000000101'::uuid,
            'main_video',
            'video',
            'en',
            'Episode 1 (English)',
            NULL,
            'brightcove',
            'bc-afs-ep01',
            NULL,
            'https://example.invalid/afs/ep-01/en.mp4',
            true,
            true,
            1680,
            NULL
        ),
        (
            'a0000000-0000-4000-a000-000000000402'::uuid,
            'a0000000-0000-4000-a000-000000000101'::uuid,
            'main_video',
            'video',
            'fr',
            'Épisode 1 (français)',
            NULL,
            'brightcove',
            'bc-afs-ep01',
            NULL,
            'https://example.invalid/afs/ep-01/fr.mp4',
            true,
            true,
            1720,
            NULL
        ),
        -- Episode 2: English only (language omitted → product content_language).
        (
            'a0000000-0000-4000-a000-000000000403'::uuid,
            'a0000000-0000-4000-a000-000000000102'::uuid,
            'main_video',
            'video',
            'en',
            'Episode 2 (English)',
            NULL,
            'brightcove',
            'bc-afs-ep02',
            NULL,
            'https://example.invalid/afs/ep-02/en.mp4',
            true,
            true,
            1860,
            NULL
        ),
        (
            'a0000000-0000-4000-a000-000000000404'::uuid,
            'a0000000-0000-4000-a000-000000000201'::uuid,
            'main_video',
            'video',
            'en',
            'Host briefing (English)',
            NULL,
            'vimeo',
            'vimeo-afs-tr01',
            NULL,
            'https://example.invalid/afs/tr-01/en.mp4',
            true,
            false,
            720,
            NULL
        ),
        -- Product-level: hero (language-neutral) and EN/FR discussion guide (group_code).
        (
            'a0000000-0000-4000-a000-000000000501'::uuid,
            NULL,
            'hero_image',
            'image',
            NULL,
            'Alpha Film Series hero',
            NULL,
            'url',
            NULL,
            NULL,
            'https://example.invalid/afs/hero.jpg',
            false,
            true,
            NULL,
            245760
        ),
        (
            'a0000000-0000-4000-a000-000000000502'::uuid,
            NULL,
            'material',
            'document',
            'en',
            'Discussion guide (English)',
            'discussion-guide',
            'url',
            NULL,
            NULL,
            'https://example.invalid/afs/guide-en.pdf',
            false,
            true,
            NULL,
            1048576
        ),
        (
            'a0000000-0000-4000-a000-000000000503'::uuid,
            NULL,
            'material',
            'document',
            'fr',
            'Guide de discussion (français)',
            'discussion-guide',
            'url',
            NULL,
            NULL,
            'https://example.invalid/afs/guide-fr.pdf',
            false,
            true,
            NULL,
            1101004
        ),
        -- Market-restricted supporting clip (za only). No asset_market rows = everywhere.
        (
            'a0000000-0000-4000-a000-000000000504'::uuid,
            'a0000000-0000-4000-a000-000000000101'::uuid,
            'supporting',
            'video',
            'en',
            'South Africa trailer',
            NULL,
            'url',
            NULL,
            'https://example.invalid/afs/ep-01/za-trailer.m3u8',
            'https://example.invalid/afs/ep-01/za-trailer.mp4',
            true,
            true,
            90,
            NULL
        )
) AS a(
    id,
    item_id,
    role,
    kind,
    language_code,
    title,
    group_code,
    provider,
    provider_asset_id,
    stream_url,
    download_url,
    allow_stream,
    allow_download,
    duration_seconds,
    file_size_bytes
)
WHERE p.code = 'alpha-film-series';

INSERT INTO asset_market (asset_id, market_code)
VALUES (
    'a0000000-0000-4000-a000-000000000504',
    'za'
);

COMMIT;

SELECT
    p.id,
    p.code,
    p.title,
    p.content_language,
    (SELECT count(*) FROM product_item i WHERE i.product_id = p.id) AS items,
    (SELECT count(*) FROM asset a WHERE a.product_id = p.id) AS assets,
    (SELECT count(*) FROM product_tag pt WHERE pt.product_id = p.id) AS tags,
    (SELECT count(*) FROM product_market pm WHERE pm.product_id = p.id) AS markets
FROM product p
WHERE p.code = 'alpha-film-series';
