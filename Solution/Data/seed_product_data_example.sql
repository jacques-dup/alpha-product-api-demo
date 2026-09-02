-- =====================================================================================
-- Example product seed for the demo Product API -- THREE courses (dossier section 2.7).
--
--   psql "$DATABASE_URL" -v ON_ERROR_STOP=1 -f product_schema.sql
--   psql "$DATABASE_URL" -v ON_ERROR_STOP=1 -f seed_product_data_example.sql
--
-- Re-runnable: upserts the lookups, replaces the three product graphs.
--
-- RUN ORDER matters if you also run the smoke test. product_schema_smoketest.sql ends by
-- deleting the markets it created ('za', 'ssa') -- market is ON DELETE RESTRICT, and this
-- seed's product_market rows reference both, so the smoke test FAILS on a seeded database.
-- Use: schema -> smoketest -> seed. (Or schema -> seed, and run the smoke test on a
-- scratch database.) This is a pre-existing limitation of the smoke test, not of the seed.
--
-- ALL DATA HERE IS SYNTHETIC. Titles and episode names are modelled on real Alpha products
-- so the demo looks credible, but every identifier, URL and file size is invented:
--   * video ids are fake (real Brightcove/Vimeo ids are not used)
--   * every URL is on a .invalid hostname, so nothing can accidentally resolve or be hotlinked
--   * no real people appear -- see "NOT MODELLED" below
--
-- THE THREE COURSES, chosen to exercise different paths through the schema
--
--   family alpha-youth-series
--     alpha-youth-series-3-en    Alpha Youth Series: Original Stories
--                                10 episodes + 4 training, provider brightcove,
--                                7 languages, dubs on ep-01..03 -> language fallback
--     alpha-youth-talks-africa   Alpha Youth Talks Africa (Africa street interviews)
--                                13 episodes, provider vimeo, English only,
--                                Africa markets only -> country ACL negative path
--   family alpha-film-series
--     alpha-film-series-43       Alpha Film Series
--                                16 episodes + 2 training, provider vimeo,
--                                7 languages, dubs on ep-01..02, adult audience
--
--   So the seed covers: two families (one with two members, one with one), both video
--   providers plus 'url', episode and training item kinds, optional and grouped items,
--   multi-language and single-language products, a product available almost everywhere and
--   one restricted to Africa, and one asset restricted below product level via asset_market.
--
-- USEFUL API CHECKS
--   GET /product/products                                  -- 3 products, 2 families
--   GET /product/products/code/alpha-youth-series-3-en     -- 14 items, the showcase
--   GET /product/products?courseType=alpha-youth-series    -- 2 products (family filter)
--   GET /product/products?audience=youth                   -- 2 products
--   GET /product/products?audience=adults                  -- 1 product
--   GET /product/products?country=gb                       -- AYS + AFS, not AYTA
--   GET /product/products?country=ke                       -- ACL maps to ssa; all 3
--   GET /product/products?country=au                        -- market exists, 0 products
--   GET /product/products?language=fr                      -- AYS ep-01..03 + AFS ep-01..02
--                                                             have FR; the rest fall back to en
--   GET /product/products?language=af                      -- declared, but no FR-style dubs:
--                                                             everything falls back to en
--
-- NOT MODELLED -- gaps this seed exposed in product_schema.sql. Worth a decision before the
-- demo, because the current MyAlpha UI shows all of these:
--   1. Product duration. The UI carries duration.sessionCount and duration.time. sessionCount
--      is derivable (count of episode items), but duration.time is the course CALENDAR length
--      (4838400s = 56 days for a 10-session course), not playback time. Nothing in the schema
--      holds it. Playback totals are derivable from asset.duration_seconds; calendar length
--      is not derivable from anything.
--   2. Image size sets. The UI has 7+ renditions per image (thumbnail, medium, medium_large,
--      large, card_short, lesson_feature, 1536/2048). `asset` holds ONE url per row. This
--      seed stores the large rendition only. Options: an asset_rendition child table, or a
--      convention that the CDN derives sizes from one url.
--   3. Video download renditions. Each real video has four download links (2160/1080/480/360)
--      with sizes. `asset` has a single download_url + file_size_bytes. This seed stores 1080p.
--   4. Presenters and contributors. The UI lists eight hosts and four production credits, with
--      photos and role titles. These are NAMED REAL INDIVIDUALS -- personal data. There is no
--      table for them and this seed deliberately contains none. If the demo needs a credits
--      panel, that is a data-protection decision (lawful basis, retention) before it is a
--      schema one; do not seed real names into a demo database.
--   5. "What's included" panel (title, body, four inline SVG icons). No schema home. Probably
--      belongs to the family, not the product.
--   6. Product ordering. MyAlpha orders products with menu_order; `product` has no sequence
--      column, only `product_family` does.
--
-- IDs: product_family, product and tag carry fixed readable UUIDs so tests can hard-code
-- them. product_item and asset take gen_random_uuid() -- they are always reached through
-- (product_id, code), never by literal id.
-- =====================================================================================

BEGIN;

-- ---------------------------------------------------------------- language
INSERT INTO language (code, is_active) VALUES
    ('en',    true),
    ('af',    true),
    ('am',    true),
    ('fr',    true),
    ('pt',    true),
    ('pt_BR', true),
    ('zh_CN', true),
    ('zh_TW', true)
ON CONFLICT (code) DO UPDATE
    SET is_active = EXCLUDED.is_active;

-- ---------------------------------------------------------------- market
-- 'au' is deliberately seeded with no products, so ?country=au exercises the empty case.
INSERT INTO market (code, kind, name) VALUES
    ('gb',  'country', 'United Kingdom'),
    ('us',  'country', 'United States'),
    ('ca',  'country', 'Canada'),
    ('za',  'country', 'South Africa'),
    ('ke',  'country', 'Kenya'),
    ('au',  'country', 'Australia'),
    ('ssa', 'region',  'Sub-Saharan Africa'),
    ('lat', 'region',  'Latin America')
ON CONFLICT (code) DO UPDATE
    SET kind = EXCLUDED.kind,
        name = EXCLUDED.name;

-- ---------------------------------------------------------------- tag
INSERT INTO tag (id, category, code, name, is_public, sequence) VALUES
    ('d0000000-0000-4000-a000-000000000001', 'audience', 'youth',       'Youth',       true, 1),
    ('d0000000-0000-4000-a000-000000000002', 'audience', 'adults',      'Adults',      true, 2),
    ('d0000000-0000-4000-a000-000000000003', 'audience', 'students',    'Students',    true, 3),
    ('d0000000-0000-4000-a000-000000000011', 'format',   'film',        'Film',        true, 1),
    ('d0000000-0000-4000-a000-000000000012', 'format',   'talks',       'Talks',       true, 2),
    ('d0000000-0000-4000-a000-000000000013', 'format',   'interviews',  'Interviews',  true, 3),
    ('d0000000-0000-4000-a000-000000000021', 'delivery', 'in-person',   'In person',   true, 1),
    ('d0000000-0000-4000-a000-000000000022', 'delivery', 'online',      'Online',      true, 2),
    ('d0000000-0000-4000-a000-000000000031', 'context',  'schools',     'Schools',     false, 1)
ON CONFLICT (category, code) DO UPDATE
    SET name      = EXCLUDED.name,
        is_public = EXCLUDED.is_public,
        sequence  = EXCLUDED.sequence;

-- ---------------------------------------------------------------- product_family
INSERT INTO product_family (id, code, name, summary, sequence) VALUES
    ('f0000000-0000-4000-a000-000000000001',
     'alpha-youth-series',
     'Alpha Youth Series',
     'A new way for young people to explore life, faith and purpose.',
     1),
    ('f0000000-0000-4000-a000-000000000002',
     'alpha-film-series',
     'Alpha Film Series',
     'A series of films that explore the Christian faith.',
     2)
ON CONFLICT (code) DO UPDATE
    SET name     = EXCLUDED.name,
        summary  = EXCLUDED.summary,
        sequence = EXCLUDED.sequence;

-- ---------------------------------------------------------------- product
-- Cascades to product_tag / product_market / product_item / asset / asset_market.
DELETE FROM product WHERE code IN (
    'alpha-youth-series-3-en',
    'alpha-youth-talks-africa',
    'alpha-film-series-43'
);

INSERT INTO product (id, family_id, code, title, summary, description, content_language) VALUES
    ('a0000000-0000-4000-a000-000000000001',
     'f0000000-0000-4000-a000-000000000001',
     'alpha-youth-series-3-en',
     'Alpha Youth Series: Original Stories',
     'A new way for young people to explore life, faith and purpose.',
     'Made for this generation, the series creatively unpacks the real questions young people have. Featuring the original story interviews produced for the series, it explores the core ideas of the Christian faith in a relevant and engaging way.',
     'en'),
    ('a0000000-0000-4000-a000-000000000002',
     'f0000000-0000-4000-a000-000000000001',
     'alpha-youth-talks-africa',
     'Alpha Youth Talks Africa (Africa street interviews)',
     'Street interviews with young people across Africa, asking the questions everyone is already thinking about.',
     'A companion set of short street-interview films, recorded across Africa, designed to open up each Alpha Youth conversation with voices from the room next door rather than the other side of the world.',
     'en'),
    ('a0000000-0000-4000-a000-000000000003',
     'f0000000-0000-4000-a000-000000000002',
     'alpha-film-series-43',
     'Alpha Film Series',
     'Sixteen films that explore the Christian faith.',
     'The Alpha Film Series was filmed around the world and features interviews with well-known leaders alongside personal stories. It creates a space to explore life, faith and God in a relaxed, non-pressured way.',
     'en');

-- ---------------------------------------------------------------- product_tag
INSERT INTO product_tag (product_id, tag_id)
SELECT p.id, t.id
FROM (VALUES
    ('alpha-youth-series-3-en',  'audience', 'youth'),
    ('alpha-youth-series-3-en',  'format',   'film'),
    ('alpha-youth-series-3-en',  'delivery', 'in-person'),
    ('alpha-youth-series-3-en',  'delivery', 'online'),
    ('alpha-youth-talks-africa', 'audience', 'youth'),
    ('alpha-youth-talks-africa', 'format',   'interviews'),
    ('alpha-youth-talks-africa', 'delivery', 'in-person'),
    ('alpha-film-series-43',     'audience', 'adults'),
    ('alpha-film-series-43',     'format',   'film'),
    ('alpha-film-series-43',     'delivery', 'in-person'),
    ('alpha-film-series-43',     'delivery', 'online')
) AS v(product_code, category, tag_code)
JOIN product p ON p.code = v.product_code
JOIN tag     t ON t.category = v.category AND t.code = v.tag_code;

-- ---------------------------------------------------------------- product_market
-- AYTA is Africa-only on purpose: it is the negative case for the country filter.
INSERT INTO product_market (product_id, market_code, launched_on)
SELECT p.id, v.market_code, v.launched_on
FROM (VALUES
    ('alpha-youth-series-3-en',  'gb',  DATE '2024-11-01'),
    ('alpha-youth-series-3-en',  'us',  DATE '2024-11-01'),
    ('alpha-youth-series-3-en',  'ca',  DATE '2024-11-01'),
    ('alpha-youth-series-3-en',  'ssa', NULL),
    ('alpha-youth-series-3-en',  'lat', NULL),
    ('alpha-youth-talks-africa', 'za',  DATE '2023-11-01'),
    ('alpha-youth-talks-africa', 'ke',  DATE '2023-11-01'),
    ('alpha-youth-talks-africa', 'ssa', NULL),
    ('alpha-film-series-43',     'gb',  DATE '2016-09-01'),
    ('alpha-film-series-43',     'us',  DATE '2016-09-01'),
    ('alpha-film-series-43',     'ca',  DATE '2016-09-01'),
    ('alpha-film-series-43',     'za',  DATE '2017-02-01'),
    ('alpha-film-series-43',     'ssa', NULL),
    ('alpha-film-series-43',     'lat', NULL)
) AS v(product_code, market_code, launched_on)
JOIN product p ON p.code = v.product_code;

-- =====================================================================================
-- ITEMS AND THEIR VIDEOS
--
-- One staging table holds every episode and training session for all three courses, with
-- the video spec alongside it, so each session's facts live in exactly one place. The
-- product_item and asset rows below are all derived from it. Dropped at COMMIT.
-- =====================================================================================
CREATE TEMP TABLE seed_item (
    product_code      text    NOT NULL,
    kind              text    NOT NULL,
    code              text    NOT NULL,
    sequence          integer NOT NULL,
    title             text    NOT NULL,
    summary           text    NULL,
    grouping          text    NULL,
    is_optional       boolean NOT NULL,
    provider          text    NOT NULL,
    provider_asset_id text    NOT NULL,
    duration_seconds  integer NOT NULL,
    media_prefix      text    NOT NULL,   -- path segment for the invented URLs
    dub_languages     text[]  NULL,       -- languages with their own dub, besides en
    PRIMARY KEY (product_code, code)
) ON COMMIT DROP;

INSERT INTO seed_item (
    product_code, kind, code, sequence, title, summary, grouping, is_optional,
    provider, provider_asset_id, duration_seconds, media_prefix, dub_languages
) VALUES
    ('alpha-youth-series-3-en', 'episode', 'ep-01', 1, 'Welcome to the Conversation', 'An invitation to join the journey. Session one creates the space to ask life''s big questions about purpose and what really matters.', NULL, false, 'brightcove', '6361000000001', 1227, 'ays', ARRAY['fr','pt']),
    ('alpha-youth-series-3-en', 'episode', 'ep-02', 2, 'Jesus Is...?', 'Who Jesus was and is, explored through history, his teachings and his goodness.', NULL, false, 'brightcove', '6361000000002', 1357, 'ays', ARRAY['fr','pt']),
    ('alpha-youth-series-3-en', 'episode', 'ep-03', 3, 'What Does Real Love Look Like?', 'The meaning of real, unconditional, sacrificial love, exploring sin, forgiveness and grace.', NULL, false, 'brightcove', '6361000000003', 1297, 'ays', ARRAY['fr']),
    ('alpha-youth-series-3-en', 'episode', 'ep-04', 4, 'Why Would I Want a Relationship With God?', 'From everyday friendships to a relationship with God, and how that changes and transforms us.', NULL, false, 'brightcove', '6361000000004', 1189, 'ays', NULL),
    ('alpha-youth-series-3-en', 'episode', 'ep-05', 5, 'What Does a Relationship With God Look Like?', 'The practical and personal ways we grow in relationship with God: prayer, the Bible and worship.', NULL, false, 'brightcove', '6361000000005', 1419, 'ays', NULL),
    ('alpha-youth-series-3-en', 'episode', 'ep-06', 6, 'Who Is the Holy Spirit?', 'The Holy Spirit, and what the Holy Spirit could do in our lives. Run together with session seven.', NULL, false, 'brightcove', '6361000000006', 1363, 'ays', NULL),
    ('alpha-youth-series-3-en', 'episode', 'ep-07', 7, 'How Can I Be Filled With the Holy Spirit?', 'How we experience the Holy Spirit and the gifts of the Spirit, with space to encounter God.', NULL, false, 'brightcove', '6361000000007', 920, 'ays', NULL),
    ('alpha-youth-series-3-en', 'episode', 'ep-08', 8, 'How Does God Help Us Overcome Evil?', 'What evil is, where we see it in the world, and what the Bible says we can do to face it.', NULL, false, 'brightcove', '6361000000008', 1220, 'ays', NULL),
    ('alpha-youth-series-3-en', 'episode', 'ep-09', 9, 'Does God Heal Today?', 'What healing is, how we play our part, and how healing is presented in the Bible.', NULL, false, 'brightcove', '6361000000009', 897, 'ays', NULL),
    ('alpha-youth-series-3-en', 'episode', 'ep-10', 10, 'What Happens Next?', 'The Church, community, and what comes after Alpha. This is the start, not the end.', NULL, false, 'brightcove', '6361000000010', 1467, 'ays', NULL),
    ('alpha-youth-series-3-en', 'training', 'tr-01', 1, 'Team Training: Hosting a Small Group', 'How to host a table so every young person gets a say.', 'training', false, 'brightcove', '6361000000901', 1080, 'ays', NULL),
    ('alpha-youth-series-3-en', 'training', 'tr-02', 2, 'Team Training: Listening Well', 'Listening as the core skill of a good Alpha host.', 'training', false, 'brightcove', '6361000000902', 960, 'ays', NULL),
    ('alpha-youth-series-3-en', 'training', 'tr-03', 3, 'Team Training: Preparing for the Weekend', 'Getting the team ready for the Alpha Youth Day.', 'weekend', true, 'brightcove', '6361000000903', 840, 'ays', NULL),
    ('alpha-youth-series-3-en', 'training', 'tr-04', 4, 'Team Training: After the Weekend', 'Keeping momentum once the weekend is over.', 'postweekend', true, 'brightcove', '6361000000904', 780, 'ays', NULL),
    ('alpha-youth-talks-africa', 'episode', 'int-01', 1, 'What Do You Live For?', NULL, NULL, false, 'vimeo', '900100001', 612, 'ayta', NULL),
    ('alpha-youth-talks-africa', 'episode', 'int-02', 2, 'Who Do You Say Jesus Is?', NULL, NULL, false, 'vimeo', '900100002', 548, 'ayta', NULL),
    ('alpha-youth-talks-africa', 'episode', 'int-03', 3, 'What Does Love Mean to You?', NULL, NULL, false, 'vimeo', '900100003', 701, 'ayta', NULL),
    ('alpha-youth-talks-africa', 'episode', 'int-04', 4, 'Have You Ever Prayed?', NULL, NULL, false, 'vimeo', '900100004', 486, 'ayta', NULL),
    ('alpha-youth-talks-africa', 'episode', 'int-05', 5, 'Is There a God?', NULL, NULL, false, 'vimeo', '900100005', 655, 'ayta', NULL),
    ('alpha-youth-talks-africa', 'episode', 'int-06', 6, 'What Happens When We Die?', NULL, NULL, false, 'vimeo', '900100006', 720, 'ayta', NULL),
    ('alpha-youth-talks-africa', 'episode', 'int-07', 7, 'Does Faith Still Matter?', NULL, NULL, false, 'vimeo', '900100007', 533, 'ayta', NULL),
    ('alpha-youth-talks-africa', 'episode', 'int-08', 8, 'Where Do You Find Hope?', NULL, NULL, false, 'vimeo', '900100008', 598, 'ayta', NULL),
    ('alpha-youth-talks-africa', 'episode', 'int-09', 9, 'What Is Forgiveness?', NULL, NULL, false, 'vimeo', '900100009', 644, 'ayta', NULL),
    ('alpha-youth-talks-africa', 'episode', 'int-10', 10, 'Have You Ever Felt Truly Seen?', NULL, NULL, false, 'vimeo', '900100010', 571, 'ayta', NULL),
    ('alpha-youth-talks-africa', 'episode', 'int-11', 11, 'What Would You Ask God?', NULL, NULL, false, 'vimeo', '900100011', 505, 'ayta', NULL),
    ('alpha-youth-talks-africa', 'episode', 'int-12', 12, 'Who Is Your Community?', NULL, NULL, false, 'vimeo', '900100012', 617, 'ayta', NULL),
    ('alpha-youth-talks-africa', 'episode', 'int-13', 13, 'What Comes Next?', NULL, NULL, false, 'vimeo', '900100013', 689, 'ayta', NULL),
    ('alpha-film-series-43', 'episode', 'ep-01', 1, 'Is There More to Life Than This?', 'An invitation to explore the big questions of life.', NULL, false, 'vimeo', '900200001', 1680, 'afs', ARRAY['fr','pt_BR']),
    ('alpha-film-series-43', 'episode', 'ep-02', 2, 'Who Is Jesus?', 'Who Jesus claimed to be, and why it matters.', NULL, false, 'vimeo', '900200002', 1860, 'afs', ARRAY['fr']),
    ('alpha-film-series-43', 'episode', 'ep-03', 3, 'Why Did Jesus Die?', 'The cross, and what Christians believe was accomplished there.', NULL, false, 'vimeo', '900200003', 1740, 'afs', NULL),
    ('alpha-film-series-43', 'episode', 'ep-04', 4, 'How Can I Have Faith?', 'What faith is, and how anyone can begin.', NULL, false, 'vimeo', '900200004', 1620, 'afs', NULL),
    ('alpha-film-series-43', 'episode', 'ep-05', 5, 'Why and How Do I Pray?', 'Prayer as conversation, and how to start.', NULL, false, 'vimeo', '900200005', 1590, 'afs', NULL),
    ('alpha-film-series-43', 'episode', 'ep-06', 6, 'Why and How Should I Read the Bible?', 'Getting into the Bible for the first time.', NULL, false, 'vimeo', '900200006', 1710, 'afs', NULL),
    ('alpha-film-series-43', 'episode', 'ep-07', 7, 'How Does God Guide Us?', 'The ways Christians describe hearing from God.', NULL, false, 'vimeo', '900200007', 1665, 'afs', NULL),
    ('alpha-film-series-43', 'episode', 'ep-08', 8, 'Who Is the Holy Spirit?', 'An introduction to the third person of the Trinity.', NULL, false, 'vimeo', '900200008', 1620, 'afs', NULL),
    ('alpha-film-series-43', 'episode', 'ep-09', 9, 'What Does the Holy Spirit Do?', 'The Spirit at work in a person''s life.', NULL, false, 'vimeo', '900200009', 1575, 'afs', NULL),
    ('alpha-film-series-43', 'episode', 'ep-10', 10, 'How Can I Be Filled With the Holy Spirit?', 'Being filled, and what people experience.', NULL, false, 'vimeo', '900200010', 1710, 'afs', NULL),
    ('alpha-film-series-43', 'episode', 'ep-11', 11, 'How Can I Resist Evil?', 'Facing evil, and where Christians find strength.', NULL, false, 'vimeo', '900200011', 1545, 'afs', NULL),
    ('alpha-film-series-43', 'episode', 'ep-12', 12, 'Why and How Should I Tell Others?', 'Talking about faith without the awkwardness.', NULL, false, 'vimeo', '900200012', 1500, 'afs', NULL),
    ('alpha-film-series-43', 'episode', 'ep-13', 13, 'Does God Heal Today?', 'Healing, honestly discussed.', NULL, false, 'vimeo', '900200013', 1635, 'afs', NULL),
    ('alpha-film-series-43', 'episode', 'ep-14', 14, 'What About the Church?', 'Why community matters.', NULL, false, 'vimeo', '900200014', 1560, 'afs', NULL),
    ('alpha-film-series-43', 'episode', 'ep-15', 15, 'How Can I Make the Most of the Rest of My Life?', 'Living differently after Alpha.', NULL, false, 'vimeo', '900200015', 1620, 'afs', NULL),
    ('alpha-film-series-43', 'episode', 'ep-16', 16, 'What Now?', 'Where to go from here.', NULL, false, 'vimeo', '900200016', 1440, 'afs', NULL),
    ('alpha-film-series-43', 'training', 'tr-01', 1, 'Team Training: Hosting a Small Group', 'Facilitating discussion for adult guests.', 'training', false, 'vimeo', '900200901', 1140, 'afs', NULL),
    ('alpha-film-series-43', 'training', 'tr-02', 2, 'Team Training: The Alpha Weekend', 'Running the Alpha Weekend well.', 'weekend', true, 'vimeo', '900200902', 1020, 'afs', NULL);

-- ---------------------------------------------------------------- product_item
INSERT INTO product_item (
    product_id, kind, code, sequence, title, summary, grouping, is_optional
)
SELECT p.id, s.kind, s.code, s.sequence, s.title, s.summary, s.grouping, s.is_optional
FROM seed_item s
JOIN product p ON p.code = s.product_code;

-- ---------------------------------------------------------------- asset: main_video (en)
-- Brightcove keeps the id and streams from the player; Vimeo streams from its own player.
-- Both get an invented 1080p download. See gap 3 in the header: real videos carry four
-- renditions, and only one fits here.
INSERT INTO asset (
    product_id, item_id, role, kind, language_code, title, group_code,
    provider, provider_asset_id, stream_url, download_url,
    allow_stream, allow_download, duration_seconds, file_size_bytes
)
SELECT
    i.product_id,
    i.id,
    'main_video',
    'video',
    'en',
    s.title || ' (English)',
    NULL,
    s.provider,
    s.provider_asset_id,
    CASE s.provider
        WHEN 'brightcove' THEN 'https://players.alpha.invalid/brightcove/' || s.provider_asset_id || '/master.m3u8'
        ELSE                    'https://player.vimeo.com.invalid/video/'  || s.provider_asset_id
    END,
    'https://media.alpha.invalid/' || s.media_prefix || '/' || s.code || '/en-1920x1080.mp4',
    true,
    true,
    s.duration_seconds,
    -- ~0.9 MB per second at 1080p, rounded; invented but internally consistent
    (s.duration_seconds * 950000)::bigint
FROM seed_item s
JOIN product      p ON p.code = s.product_code
JOIN product_item i ON i.product_id = p.id AND i.code = s.code;

-- ---------------------------------------------------------------- asset: main_video (dubs)
-- Same item_id, different language_code. This is the row set that makes ?language=fr
-- return a real dub for some episodes and fall back to English for the rest.
INSERT INTO asset (
    product_id, item_id, role, kind, language_code, title, group_code,
    provider, provider_asset_id, stream_url, download_url,
    allow_stream, allow_download, duration_seconds, file_size_bytes
)
SELECT
    i.product_id,
    i.id,
    'main_video',
    'video',
    dub.lang,
    s.title || ' (' || dub.lang || ')',
    NULL,
    s.provider,
    s.provider_asset_id,          -- same source video, dubbed audio track
    CASE s.provider
        WHEN 'brightcove' THEN 'https://players.alpha.invalid/brightcove/' || s.provider_asset_id || '/master.m3u8?lang=' || dub.lang
        ELSE                    'https://player.vimeo.com.invalid/video/'  || s.provider_asset_id || '?lang=' || dub.lang
    END,
    'https://media.alpha.invalid/' || s.media_prefix || '/' || s.code || '/' || dub.lang || '-1920x1080.mp4',
    true,
    true,
    -- dubs run slightly long; keeps durations from looking copy-pasted
    s.duration_seconds + 40,
    ((s.duration_seconds + 40) * 950000)::bigint
FROM seed_item s
CROSS JOIN LATERAL unnest(s.dub_languages) AS dub(lang)
JOIN product      p ON p.code = s.product_code
JOIN product_item i ON i.product_id = p.id AND i.code = s.code
WHERE s.dub_languages IS NOT NULL;

-- ---------------------------------------------------------------- asset: item thumbnails
-- Language-neutral, so language_code stays NULL. One url only -- see gap 2 in the header.
INSERT INTO asset (
    product_id, item_id, role, kind, language_code, title, group_code,
    provider, provider_asset_id, stream_url, download_url,
    allow_stream, allow_download, duration_seconds, file_size_bytes
)
SELECT
    i.product_id,
    i.id,
    'thumbnail',
    'image',
    NULL,
    s.title || ' thumbnail',
    NULL,
    'url',
    NULL,
    NULL,
    'https://media.alpha.invalid/' || s.media_prefix || '/' || s.code || '/thumbnail-1024x576.jpg',
    false,
    true,
    NULL,
    104400
FROM seed_item s
JOIN product      p ON p.code = s.product_code
JOIN product_item i ON i.product_id = p.id AND i.code = s.code;

-- ---------------------------------------------------------------- asset: discussion guides
-- Alpha Youth Series only: a per-episode PDF, grouped so a client can collect the set.
-- EN plus FR on the first three, matching where the dubs are.
INSERT INTO asset (
    product_id, item_id, role, kind, language_code, title, group_code,
    provider, provider_asset_id, stream_url, download_url,
    allow_stream, allow_download, duration_seconds, file_size_bytes
)
SELECT
    i.product_id,
    i.id,
    'material',
    'document',
    g.lang,
    s.title || ' // Discussion Questions'
        || CASE g.lang WHEN 'en' THEN '' ELSE ' (' || g.lang || ')' END,
    'discussion-guide',
    'url',
    NULL,
    NULL,
    'https://media.alpha.invalid/ays/' || s.code || '/discussion-guide-' || g.lang || '.pdf',
    false,
    true,
    NULL,
    49152
FROM seed_item s
JOIN product      p ON p.code = s.product_code
JOIN product_item i ON i.product_id = p.id AND i.code = s.code
CROSS JOIN LATERAL (
    SELECT unnest(
        CASE WHEN s.sequence <= 3 THEN ARRAY['en','fr'] ELSE ARRAY['en'] END
    ) AS lang
) g
WHERE s.product_code = 'alpha-youth-series-3-en'
  AND s.kind = 'episode';

-- ---------------------------------------------------------------- asset: weekly briefings
-- Alpha Youth Series only: a short leader briefing video per episode. role 'supporting'
-- keeps it out of the way of main_video while still hanging off the same item.
INSERT INTO asset (
    product_id, item_id, role, kind, language_code, title, group_code,
    provider, provider_asset_id, stream_url, download_url,
    allow_stream, allow_download, duration_seconds, file_size_bytes
)
SELECT
    i.product_id,
    i.id,
    'supporting',
    'video',
    'en',
    'Alpha Youth Weekly Briefing: ' || s.title,
    'weekly-briefing',
    'brightcove',
    '6362' || lpad(s.sequence::text, 9, '0'),
    'https://players.alpha.invalid/brightcove/6362' || lpad(s.sequence::text, 9, '0') || '/master.m3u8',
    'https://media.alpha.invalid/ays/' || s.code || '/weekly-briefing-en-1920x1080.mp4',
    true,
    true,
    90 + s.sequence * 7,
    ((90 + s.sequence * 7) * 950000)::bigint
FROM seed_item s
JOIN product      p ON p.code = s.product_code
JOIN product_item i ON i.product_id = p.id AND i.code = s.code
WHERE s.product_code = 'alpha-youth-series-3-en'
  AND s.kind = 'episode';

-- =====================================================================================
-- PRODUCT-LEVEL ASSETS (item_id NULL): hero, card thumbnail, teaser, material packs.
-- =====================================================================================
INSERT INTO asset (
    product_id, item_id, role, kind, language_code, title, group_code,
    provider, provider_asset_id, stream_url, download_url,
    allow_stream, allow_download, duration_seconds, file_size_bytes
)
SELECT
    p.id, NULL, v.role, v.kind, v.language_code, v.title, v.group_code,
    v.provider, v.provider_asset_id, v.stream_url, v.download_url,
    v.allow_stream, v.allow_download, v.duration_seconds, v.file_size_bytes
FROM (VALUES
    -- Alpha Youth Series: Original Stories -------------------------------------------
    ('alpha-youth-series-3-en', 'hero_image', 'image', NULL, 'Alpha Youth Series hero banner', NULL,
     'url', NULL, NULL,
     'https://media.alpha.invalid/ays/hero-1600x270.jpg',
     false, true, NULL, 504832::bigint),
    ('alpha-youth-series-3-en', 'thumbnail', 'image', NULL, 'Alpha Youth Series cover', NULL,
     'url', NULL, NULL,
     'https://media.alpha.invalid/ays/cover-1024x898.jpg',
     false, true, NULL, 283800::bigint),
    ('alpha-youth-series-3-en', 'promo_video', 'video', 'en', 'Alpha Youth Series teaser', NULL,
     'brightcove', '6361400000001',
     'https://players.alpha.invalid/brightcove/6361400000001/master.m3u8',
     'https://media.alpha.invalid/ays/teaser-en-1920x1080.mp4',
     true, false, 96, 91200000::bigint),
    ('alpha-youth-series-3-en', 'promo_banner', 'image', NULL, 'Alpha Youth Series promo banner', 'promo-pack',
     'url', NULL, NULL,
     'https://media.alpha.invalid/ays/promo-banner-1050x600.jpg',
     false, true, NULL, 197979::bigint),
    ('alpha-youth-series-3-en', 'material', 'document', 'en', 'Alpha Youth Team Guidelines (English)', 'team-guidelines',
     'url', NULL, NULL,
     'https://media.alpha.invalid/ays/team-guidelines-en.pdf',
     false, true, NULL, 1548288::bigint),
    ('alpha-youth-series-3-en', 'material', 'document', 'fr', 'Alpha Youth Team Guidelines (French)', 'team-guidelines',
     'url', NULL, NULL,
     'https://media.alpha.invalid/ays/team-guidelines-fr.pdf',
     false, true, NULL, 1601536::bigint),

    -- Alpha Youth Talks Africa ---------------------------------------------------------
    ('alpha-youth-talks-africa', 'hero_image', 'image', NULL, 'Alpha Youth Talks Africa hero banner', NULL,
     'url', NULL, NULL,
     'https://media.alpha.invalid/ayta/hero-1600x270.jpg',
     false, true, NULL, 421888::bigint),
    ('alpha-youth-talks-africa', 'thumbnail', 'image', NULL, 'Alpha Youth Talks Africa cover', NULL,
     'url', NULL, NULL,
     'https://media.alpha.invalid/ayta/cover-768x423.jpg',
     false, true, NULL, 48761::bigint),
    ('alpha-youth-talks-africa', 'promo_video', 'video', 'en', 'Alpha Youth Talks Africa teaser', NULL,
     'vimeo', '900100000',
     'https://player.vimeo.com.invalid/video/900100000',
     NULL,
     true, false, 74, NULL),

    -- Alpha Film Series ----------------------------------------------------------------
    ('alpha-film-series-43', 'hero_image', 'image', NULL, 'Alpha Film Series hero banner', NULL,
     'url', NULL, NULL,
     'https://media.alpha.invalid/afs/hero-1600x270.jpg',
     false, true, NULL, 466944::bigint),
    ('alpha-film-series-43', 'thumbnail', 'image', NULL, 'Alpha Film Series cover', NULL,
     'url', NULL, NULL,
     'https://media.alpha.invalid/afs/cover-1024x1024.png',
     false, true, NULL, 64528::bigint),
    ('alpha-film-series-43', 'promo_video', 'video', 'en', 'Alpha Film Series teaser', NULL,
     'vimeo', '900200000',
     'https://player.vimeo.com.invalid/video/900200000',
     'https://media.alpha.invalid/afs/teaser-en-1920x1080.mp4',
     true, false, 128, 121600000::bigint),
    ('alpha-film-series-43', 'material', 'document', 'en', 'Alpha Film Series Discussion Guide (English)', 'discussion-guide',
     'url', NULL, NULL,
     'https://media.alpha.invalid/afs/discussion-guide-en.pdf',
     false, true, NULL, 1048576::bigint),
    ('alpha-film-series-43', 'material', 'document', 'fr', 'Alpha Film Series Discussion Guide (French)', 'discussion-guide',
     'url', NULL, NULL,
     'https://media.alpha.invalid/afs/discussion-guide-fr.pdf',
     false, true, NULL, 1101004::bigint),
    ('alpha-film-series-43', 'material', 'document', 'pt_BR', 'Alpha Film Series Discussion Guide (Brazilian Portuguese)', 'discussion-guide',
     'url', NULL, NULL,
     'https://media.alpha.invalid/afs/discussion-guide-pt_BR.pdf',
     false, true, NULL, 1085440::bigint)
) AS v(
    product_code, role, kind, language_code, title, group_code,
    provider, provider_asset_id, stream_url, download_url,
    allow_stream, allow_download, duration_seconds, file_size_bytes
)
JOIN product p ON p.code = v.product_code;

-- ---------------------------------------------------------------- asset_market
-- Almost every asset has NO asset_market rows, which means "available everywhere". This
-- one promo banner is restricted to za and ke so the below-product-level restriction path
-- has something to exercise: the product is listed in gb, but this asset is not.
INSERT INTO asset_market (asset_id, market_code)
SELECT a.id, m.code
FROM asset a
JOIN product p ON p.id = a.product_id
CROSS JOIN (VALUES ('za'), ('ke')) AS m(code)
WHERE p.code = 'alpha-youth-series-3-en'
  AND a.role = 'promo_banner';

COMMIT;

-- =====================================================================================
-- VERIFY -- expected shape:
--
--  family              | code                     | lang | items | eps | assets | tags | markets
--  alpha-youth-series  | alpha-youth-series-3-en  | en   |    14 |  10 |     62 |    4 |       5
--  alpha-youth-series  | alpha-youth-talks-africa | en   |    13 |  13 |     29 |    3 |       3
--  alpha-film-series   | alpha-film-series-43     | en   |    18 |  16 |     45 |    4 |       6
--
-- And languages_with_video:
--  alpha-youth-series-3-en  | {en,fr,pt}
--  alpha-youth-talks-africa | {en}
--  alpha-film-series-43     | {en,fr,pt_BR}
-- =====================================================================================
SELECT
    f.code AS family,
    p.code,
    p.content_language AS lang,
    (SELECT count(*) FROM product_item i WHERE i.product_id = p.id) AS items,
    (SELECT count(*) FROM product_item i WHERE i.product_id = p.id AND i.kind = 'episode') AS eps,
    (SELECT count(*) FROM asset a         WHERE a.product_id = p.id) AS assets,
    (SELECT count(*) FROM product_tag t   WHERE t.product_id = p.id) AS tags,
    (SELECT count(*) FROM product_market m WHERE m.product_id = p.id) AS markets
FROM product p
JOIN product_family f ON f.id = p.family_id
ORDER BY f.sequence, p.code;

-- Languages that actually have a main_video, per product -- what siteLanguages should return.
SELECT p.code,
       array_agg(DISTINCT a.language_code ORDER BY a.language_code) AS languages_with_video
FROM product p
JOIN asset a ON a.product_id = p.id AND a.role = 'main_video'
GROUP BY p.code
ORDER BY p.code;
